/*
    Made by: Pinzu
    Requirements:
        None
     
    Optional
        Table by Bribe
        PlayerUtils by TriggerHappy
     
    Note that changing extended tooltip after a cooldown has started may lead to undesired behaviour.
*/
library ShowAbilityCooldown requires /*AbilityIndexer,*/ optional Table, optional PlayerUtils
    globals   
        private constant string COOLDOWN_COLOR                = "|c00959697"                 
        private constant string COOLDOWN_LABEL                = "|n|n" + COOLDOWN_COLOR + "Cooldown: "
       
        // This will only apply cooldown tooltip locally, meaning only the owning player will see it.
        // The benefit of this is that multiple players can have the same hero type without affecting each other. 
        // The drawback is that the cooldown is not shown for shared players. 
        private constant boolean LOCAL_CHANGES                = true       
    endglobals
    struct AbilityCooldownManager
        private static constant real MIN_REQ_DELAY     = 0.8 // Must for pitlord spells, 0.7 for keeper of the grove and 0.5 for most other spells
        private static constant real REFRESH_RATE     = 1.0
       
        static if LIBRARY_Table then
            private static Table table
        else
            private static hashtable hash
        endif
       
        private unit u
        private integer abilCode
        private timer t
        private string tooltip
        private integer level
       
        private static method create takes nothing returns thistype 
            return .allocate()
        endmethod
       
        /*   
            Here you can filter out units that you wish to exclude from the system
        */
        private static method filter takes nothing returns boolean 
            local unit u = GetFilterUnit()
            if not IsUnitType(u, UNIT_TYPE_HERO) then    // example
                return false
            endif
            return true
        endmethod 
 
        private static method formatTime takes real time returns string
            return I2S(R2I(time))    // format time
        endmethod
       
        private method update takes real timeLeft  returns nothing
            local integer lvl = GetUnitAbilityLevel(.u, .abilCode)
            local player p
            static if LIBRARY_PlayerUtil then
                local User user = User.first
            else         
                local integer i = 0
            endif
   
            // if new level save text
            if .level != lvl then
                set .level = lvl
                set .tooltip = BlzGetAbilityExtendedTooltip(.abilCode, .level)
            endif
         
            // Update
            if timeLeft > 0.1 then
                if GetOwningPlayer(.u) == GetLocalPlayer() or not LOCAL_CHANGES then 
                    call BlzSetAbilityExtendedTooltip(.abilCode, .tooltip + COOLDOWN_LABEL + thistype.formatTime(timeLeft), .level)   
                endif
            else
                if GetOwningPlayer(.u) == GetLocalPlayer() or not LOCAL_CHANGES then 
                    call BlzSetAbilityExtendedTooltip(.abilCode, .tooltip, .level)
                endif
            endif
         
            // Refresh unit selection
            static if LIBRARY_PlayerUtil then
                loop // only loop through players that are playing
                    exitwhen user == User.NULL
                    if IsUnitSelected(.u, user.p) and User.fromLocal().id == user.id then
                        call SelectUnitRemove(.u)
                        call SelectUnitAdd(.u)
                    endif
                    set user = user.next
                endloop
            else
                loop
                    exitwhen i == bj_MAX_PLAYER_SLOTS
                    set p = Player(i)
                    if IsUnitSelected(.u, p) and GetLocalPlayer() == p then
                        call SelectUnitRemove(.u)
                        call SelectUnitAdd(.u)
                    endif
                    set i = i + 1
                endloop
                set p = null
            endif
         
            // Clean up if finished
            if timeLeft <= 0.1 then
                static if LIBRARY_Table then
                    call thistype.table.remove(GetHandleId(.t))
                else
                    call FlushChildHashtable(thistype.hash, GetHandleId(.t))
                endif
                // Ability Indexer if its a unit...
                //call DeindexUnitAbilityLevel(.u, abilCode)
                call PauseTimer(.t)
                call DestroyTimer(.t)
                set .t = null
                set .u = null
                call .deallocate()
            endif
        endmethod
     
        private static method onTimerExpires takes nothing returns nothing
            static if LIBRARY_Table then
                local thistype this = thistype.table[GetHandleId(GetExpiredTimer())]
            else
                local thistype this = LoadInteger(thistype.hash, GetHandleId(GetExpiredTimer()), 0)
            endif
            local real cooldown = BlzGetUnitAbilityCooldownRemaining(this.u, this.abilCode)
            call this.update(BlzGetUnitAbilityCooldownRemaining(.u, .abilCode))
        endmethod
        private static method addToPool takes nothing returns nothing 
            static if LIBRARY_Table then
                local thistype this = thistype.table[GetHandleId(GetExpiredTimer())]
            else
                local thistype this = LoadInteger(thistype.hash, GetHandleId(GetExpiredTimer()), 0)
            endif
           call this.update(BlzGetUnitAbilityCooldownRemaining(this.u, this.abilCode))
           call TimerStart(this.t, thistype.REFRESH_RATE, true, function thistype.onTimerExpires)
        endmethod
   
        private static method adjustForOffset takes nothing returns nothing 
            static if LIBRARY_Table then
                local thistype this = thistype.table[GetHandleId(GetExpiredTimer())]
            else
                local thistype this = LoadInteger(thistype.hash, GetHandleId(GetExpiredTimer()), 0)
            endif
            local real cooldown = BlzGetUnitAbilityCooldownRemaining(this.u, this.abilCode)
            call TimerStart(this.t, cooldown - R2I(cooldown), false, function thistype.addToPool)
        endmethod
   
        public static method start takes unit u, integer abilCode returns nothing
            local thistype this 
            local real cooldown = BlzGetUnitAbilityCooldown(u, abilCode, GetUnitAbilityLevel(u, abilCode))
            if cooldown == 0. then 
                return
            endif
            set this = .allocate()
            set this.abilCode = abilCode
            set this.u = u
            set this.level = -1
            set this.t = CreateTimer()
            // Ability Indexer
            //call IndexUnitAbilityLevel(u, abilCode)
            call TimerStart(this.t, thistype.MIN_REQ_DELAY, false, function thistype.adjustForOffset)
            static if LIBRARY_Table then
                set thistype.table[GetHandleId(this.t)] = this
            else
                call SaveInteger(thistype.hash, GetHandleId(this.t), 0, this)
            endif
            call this.update(cooldown)
        endmethod
       
        static method onSpellFinish takes nothing returns boolean
            call thistype.start(GetTriggerUnit(), GetSpellAbilityId())
            return false
        endmethod
       
        private static method onInit takes nothing returns nothing
            local trigger trgSpell = CreateTrigger()
            //local trigger trgDeath = CreateTrigger()
            static if LIBRARY_PlayerUtil then
                local User p = User.first
                loop // only loop through players that are playing
                    exitwhen p == User.NULL
                    call  TriggerRegisterPlayerUnitEvent(trgSpell, p.p, EVENT_PLAYER_UNIT_SPELL_CAST, null)
                    set p = p.next
                endloop
            else
                local integer i = 0
                local player p
                loop
                    exitwhen i == bj_MAX_PLAYER_SLOTS
                    set p = Player(i)
                    if (GetPlayerSlotState(p) == PLAYER_SLOT_STATE_PLAYING and /*
                    */ GetPlayerController(p) == MAP_CONTROL_USER) then
                        call  TriggerRegisterPlayerUnitEvent(trgSpell, p, EVENT_PLAYER_UNIT_SPELL_CAST, Filter(function thistype.filter))
                    endif
                    set i = i + 1
                endloop
                set p = null
            endif
            call TriggerAddCondition(trgSpell, Condition(function thistype.onSpellFinish))
            static if LIBRARY_Table then
                set thistype.table = Table.create()
            else
                set thistype.hash = InitHashtable()
            endif
            set trgSpell = null
           
            // Add Unit Abilities that should be indexed...
            //call AddAbilityToIndexer('Ahea', 100)
           
        endmethod
    endstruct
endlibrary