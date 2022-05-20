library BuilderTrack /* v1.3 hiveworkshop.com/threads/buildertrack.290327/


    */ requires UnitDex       /* hiveworkshop.com/threads/system-unitdex-unit-indexer.248209/
 
   
    Information
    ¯¯¯¯¯¯¯¯¯¯¯
   
            Some powers to work with builders that are constructing/repairing.
         
         
    Applied method
    ¯¯¯¯¯¯¯¯¯¯¯¯¯¯
 
        Default repair mechanics:
     
        There exist always 2 similar ways for repairing and for each race.
     
            Human:
                1) The unit casts "Repair" on the building (will order "repair" internaly)
                   When reaching the building the worker will cast spell "Repair".
                2) User "right clicks" (smart-order) on the building
                   When reaching the building the worker will cast spell "Repair".
                 
                    repair-order-id: 852024
                    Repair-spell-id: 'Ahrp'
                 
            Orc:
                1) The unit casts "Repair" on the building (will order "repair" internaly)
                   When reaching the building the worker will cast spell "Repair".
                2) User "right clicks" (smart-order) on the building
                   When reaching the building the worker will cast spell "Repair".
                 
                    repair-order-id: 852024
                    Repair-spell-id: 'Arep'
                 
            Night Elves:
                1) The unit casts "Renew" on the building (will order "renew" internaly)
                   When reaching the building the worker will cast spell "Renew".
                2) User "right clicks" (smart-order) on the building
                   When reaching the building the worker will cast spell "Renew".
                 
                    renew-order-id: 852161
                    Renew-spell-id: 'Aren'
                 
            Undead:
                1) The unit casts "Restore" on the building (will order "restoration" internaly)
                   When reaching the building the worker will cast spell "Restore".
                2) User "right clicks" (smart-order) on the building
                   When reaching the building the worker will cast spell "Restore".
                 
                    restoration-order-id: 852202
                    Restore-spell-id:     'Arst'
                 
 
        Default build mechanics:
     
            Human:

                1. Worker gets point order with id of BuildingType.
                2. When worker starts constructing the building it gets the "repair" order towards the building.
                3. When receiving the "repair" order it starts the spell "Repair" towards the building.
                 
            Orc:
         
                1. Worker gets point order with id of BuildingType.
                2. < no more orders >
             
            Night Elves:
         
                1. Worker gets point order with id of BuildingType.
                2. < no more orders >
             
            Undead:
         
                1. Worker gets point order with id of BuildingType.
                2. < no more orders >
             
-----------
       
            Only for human builder (peasants) there would exist a proper method to get
            the worker who starts constructing a building, because we could detect the first unit
            who starts casting "Repair" on the building. Though, for other races there are different
            mechanics, so it's sadly not applyable.
       
            So to get the main builder this system uses a naive approach, which is not water-proof,
            but which should work in 99,9% of cases. When a structure is started being built, it
            takes the clostest worker of the respective player, which has currently the required order id.
         
            Tracking for power building (multiple units building at same time), and tracking repairing workers should work always fine.
         
*/  

//! novjass
 //=================== --------- API --------- ====================

    struct Builder
 
        public static method GetMainBuilder takes unit building returns unit
            // who initiated the construction of a building
            // (Human-race!) when a worker stops constructung, and an other one
            // starts again, the new worker will count as initiator
         
        public static method GetBuilderGroup takes unit building returns group
            // returns group with units who are currently repairing/building
            // do not destroy this group
         
        public static method GetBuilderAmount takes unit building returns integer
            // how many builders are currently repairing/building
     
        public static method IsBuildingCurrentlyConstructed takes unit building returns boolean
            // won't return "true" if a building is only being repaired
 
 //! endnovjass

struct Repair extends array
    public static constant integer HUMAN_SPELL           = 'Ahrp'
    public static constant integer HUMAN_ORDER           = 852024
    public static constant integer HUMAN_ORDER_ON        = 852025
    public static constant integer HUMAN_ORDER_OFF       = 852026
 
    public static constant integer ORC_SPELL             = 'Arep'
    public static constant integer ORC_ORDER             = 852024
    public static constant integer ORC_ORDER_ON          = 852025
    public static constant integer ORC_ORDER_OFF         = 852026
 
    public static constant integer NIGHT_ELVES_SPELL     = 'Aren'
    public static constant integer NIGHT_ELVES_ORDER     = 852161
    public static constant integer NIGHT_ELVES_ORDER_ON  = 852162
    public static constant integer NIGHT_ELVES_ORDER_OFF = 852163
 
    public static constant integer UNDEAD_SPELL          = 'Arst'
    public static constant integer UNDEAD_ORDER          = 852202
    public static constant integer UNDEAD_ORDER_ON       = 852203
    public static constant integer UNDEAD_ORDER_OFF      = 852204
 
    public static method IsRepairAbility takes integer id returns boolean
        return id == HUMAN_SPELL or id == ORC_SPELL or id == NIGHT_ELVES_SPELL or id == UNDEAD_SPELL
    endmethod
 
    public static method IsRepairOrder takes integer id returns boolean
        return id == HUMAN_ORDER or id == ORC_ORDER or id == NIGHT_ELVES_ORDER or id == UNDEAD_ORDER
    endmethod
 
    public static method IsRepairOrderOn takes integer id returns boolean
        return id == HUMAN_ORDER_ON or id == ORC_ORDER_ON or id == NIGHT_ELVES_ORDER_ON or id == UNDEAD_ORDER_ON
    endmethod
 
    public static method IsRepairOrderOff takes integer id returns boolean
        return id == HUMAN_ORDER_OFF or id == ORC_ORDER_OFF or id == NIGHT_ELVES_ORDER_OFF or id == UNDEAD_ORDER_OFF
    endmethod
endstruct
native UnitAlive            takes unit id                               returns boolean
struct Builder extends array
 
    private static constant integer SMART = 851971
    private static group Group = CreateGroup()

// building-related
    private static boolean array isConstructing
    private static integer array workerCount
    private static unit array mainBuilder
    private static group array builderGroup

// builder-related
    private static boolean array isWorking
    readonly unit builder
    readonly unit building
    readonly integer buildingType
 
    private static method create takes unit u, unit b returns thistype
        local thistype this = GetUnitId(u)
        local integer id    = GetUnitId(b)
        set .builder        = u
        set .building       = b
        set .buildingType   = GetUnitTypeId(b)
        set isWorking[this] = true
        call GroupAddUnit(builderGroup[id], u)
        if isConstructing[id] and workerCount[id] == 0 then
            set mainBuilder[id] = u
        endif
        set workerCount[id] = workerCount[id] + 1
        return this
    endmethod
 
    method destroy takes nothing returns nothing
        local integer id = GetUnitId(.building)
        call GroupRemoveUnit(builderGroup[id], .builder)
        set workerCount[id] = workerCount[id] - 1
        if isConstructing[id] and .builder == mainBuilder[id] and workerCount[id] > 0 then
            set mainBuilder[id] = FirstOfGroup(builderGroup[id])
        endif
        set .builder          = null
        set .building         = null
        set .isWorking[this]  = false
    endmethod
 
    private static method onCast takes nothing returns boolean
        local unit builder    = GetTriggerUnit()
        local unit building   = GetSpellTargetUnit()
        local integer id      = GetUnitId(building)
        if Repair.IsRepairAbility(GetSpellAbilityId()) and not IsUnitInGroup(builder, builderGroup[id]) then
            //if TriggerEvaluate(FILTER_HANDLER) then
                call create(builder, building)
            //endif
        endif
        set builder = null
        set building = null
        return false
    endmethod
 
    // if unit gets ordered we remove the instance
    private static method onOrder takes nothing returns boolean
        local unit u = GetTriggerUnit()
        local thistype this = GetUnitId(u)
        local integer order = GetIssuedOrderId()
     
        // allowed scenarios for not to destroy:
     
        // unit is ordered autorepairOn
        // unit is ordered autorepairOff while having a "smart" order (because of "smart" it will still continue repairing)
        // unit is ordered to cast a Repair Ability on the building
     
        if isWorking[this] and not Repair.IsRepairOrderOn(order) and not(Repair.IsRepairOrderOff(order) and GetUnitCurrentOrder(u) == SMART) /*
         */and not (GetOrderTarget() == this.building and Repair.IsRepairAbility(order) ) then
            call .destroy()
        endif
        set u = null
        return false
    endmethod
 
    // destroy enum units
    private static method callback takes nothing returns nothing
        call thistype(GetUnitId(GetEnumUnit())).destroy()
    endmethod
 
    private static method onRemove takes integer id returns nothing
        if builderGroup[id] != null then
            call ForGroup(builderGroup[id], function thistype.callback)
            call DestroyGroup(builderGroup[id])
            set builderGroup[id] = null
        elseif isWorking[id] then
            call thistype(id).destroy()
        endif
    endmethod
    private static method onDeindex takes nothing returns nothing
        call onRemove(GetIndexedUnitId())
    endmethod
    private static method onDeath takes nothing returns boolean
        call onRemove(GetUnitId(GetTriggerUnit()))
        return false
    endmethod
 
    private static method onIndex takes nothing returns nothing
        local integer id = GetIndexedUnitId()
        set mainBuilder[id] = null
        set isConstructing[id] = false
    endmethod
 
    private static integer orderId_s // s_ static
    private static method unitFilter takes nothing returns boolean
        local real orderId
        set bj_lastCreatedUnit = GetFilterUnit()
        set orderId = GetUnitCurrentOrder(bj_lastCreatedUnit)    // use slot orders 1-6
        return UnitAlive(bj_lastCreatedUnit) and orderId == orderId_s or (orderId >= 852008 and orderId <= 852013 )
    endmethod
 
    private static method onConstructStart takes nothing returns boolean
        local unit building = GetConstructingStructure()
        local unit builder = null
        local unit temp_unit
        local real orderId
        local unit fog
        local integer id = GetUnitId(building)
        local real x = GetUnitX(building)
        local real y = GetUnitY(building)
        local real dx
        local real dy
     
        local real distance_min = 9999
        local real distance_temp
     
        set orderId_s = GetUnitTypeId(building)
     
        set isConstructing[id] = true
        set builderGroup[id]   = CreateGroup()
        set workerCount[id]    = 0
        set mainBuilder[id] = null
     
    // night elves Gold Mine, just find nearest main house
        if GetUnitAbilityLevel(building, 'Aenc') > 0 then
         
            call GroupEnumUnitsOfPlayer(Group, GetTriggerPlayer(), null)
            loop
                set fog = FirstOfGroup(Group)
                exitwhen fog == null
             
                if GetUnitAbilityLevel(fog, 'Aent')  > 0 then
                    set dx = GetUnitX(fog) - x
                    set dy = GetUnitY(fog) - y
                    set distance_temp = SquareRoot(dx*dx + dy*dy)
                 
                    if distance_temp < distance_min then
                        set builder = fog
                    endif
                endif
             
                call GroupRemoveUnit(Group, fog)
            endloop
         
            call create(builder, building)
            set builder = null
            set building = null
            return false
        endif
       
        // find any (random) unit with current order id, because we wanna know it's type of Repair ability
        call GroupEnumUnitsOfPlayer(Group, GetTriggerPlayer(), Filter(function thistype.unitFilter))
        set temp_unit = FirstOfGroup(Group)
 
 
        if GetUnitAbilityLevel(temp_unit, Repair.HUMAN_SPELL) > 0 or GetUnitAbilityLevel(temp_unit, Repair.UNDEAD_SPELL) > 0 then
         
            // undead/human Repair -> just get closest
            loop
                set fog = FirstOfGroup(Group)
                exitwhen fog == null
             
                set dx = GetUnitX(fog) - x
                set dy = GetUnitY(fog) - y
                set distance_temp = SquareRoot(dx*dx + dy*dy)
             
                if distance_temp < distance_min then
                    set builder = fog
                endif
             
                call GroupRemoveUnit(Group, fog)
            endloop
     
        elseif GetUnitAbilityLevel(temp_unit, Repair.ORC_SPELL) > 0 or GetUnitAbilityLevel(temp_unit, Repair.NIGHT_ELVES_SPELL) > 0 then
         
            // orc/night elve Repair -> get closest hidden unit
            loop
                set fog = FirstOfGroup(Group)
                exitwhen fog == null
             
                if IsUnitHidden(fog) then
                    set dx = GetUnitX(fog) - x
                    set dy = GetUnitY(fog) - y
                    set distance_temp = SquareRoot(dx*dx + dy*dy)
                 
                    if distance_temp < distance_min then
                        set builder = fog
                    endif
                endif
             
                call GroupRemoveUnit(Group, fog)
            endloop
        endif
     
    // no luck yet, it means the building was casted via item slot (tiny building spells)
        if builder == null then
         
            loop
                set fog = FirstOfGroup(Group)
                exitwhen fog == null
             
                set orderId = GetUnitCurrentOrder(fog)
                if (orderId >= 852008 and orderId <= 852013 ) then
                    set dx = GetUnitX(fog) - x
                    set dy = GetUnitY(fog) - y
                    set distance_temp = SquareRoot(dx*dx + dy*dy)
                 
                    if distance_temp < distance_min then
                        set builder = fog
                    endif
                endif
             
                call GroupRemoveUnit(Group, fog)
            endloop
         
        endif
     
        call create(builder, building)
        set builder = null
        set building = null
        return false
    endmethod
 
    private static method onConstructFinish takes nothing returns boolean
        set isConstructing[GetUnitId(GetConstructedStructure())] = false
        return false
    endmethod
 
    implement optional BuilderEvent
 
    private static method onInit takes nothing returns nothing
        local trigger t1 = CreateTrigger()
        local trigger t2 = CreateTrigger()
        local trigger t3 = CreateTrigger()
        local trigger t4 = CreateTrigger()
        local trigger t5 = CreateTrigger()
        local player p
        local integer i = 0
        loop
            exitwhen i == bj_MAX_PLAYER_SLOTS
            set p = Player(i)
            call TriggerRegisterPlayerUnitEvent(t5, p, EVENT_PLAYER_UNIT_ISSUED_ORDER, null)
            call TriggerRegisterPlayerUnitEvent(t5, p, EVENT_PLAYER_UNIT_ISSUED_POINT_ORDER, null)
            call TriggerRegisterPlayerUnitEvent(t5, p, EVENT_PLAYER_UNIT_ISSUED_TARGET_ORDER, null)
         
            call TriggerRegisterPlayerUnitEvent(t1, p, EVENT_PLAYER_UNIT_CONSTRUCT_START, null)
            call TriggerRegisterPlayerUnitEvent(t2, p, EVENT_PLAYER_UNIT_CONSTRUCT_FINISH, null)
         
            call TriggerRegisterPlayerUnitEvent(t3, p, EVENT_PLAYER_UNIT_SPELL_EFFECT, null)
         
            call TriggerRegisterPlayerUnitEvent(t4, p, EVENT_PLAYER_UNIT_DEATH, null)
            set i = i + 1
        endloop
     
        call TriggerAddCondition(t1, Condition(function thistype.onConstructStart))
        call TriggerAddCondition(t2, Condition(function thistype.onConstructFinish))
        call TriggerAddCondition(t3, Condition(function thistype.onCast))
        call TriggerAddCondition(t4, Condition(function thistype.onDeath))
        call TriggerAddCondition(t5, Condition(function thistype.onOrder))
        call OnUnitDeindex(function thistype.onDeindex)
        call OnUnitIndex(function thistype.onIndex)
    endmethod
 
    public static method GetMainBuilder takes unit building returns unit
        return mainBuilder[GetUnitId(building)]
    endmethod
 
    public static method GetBuilderAmount takes unit building returns integer
        return workerCount[GetUnitId(building)]
    endmethod
 
    public static method GetBuilderGroup takes unit building returns group
        return builderGroup[GetUnitId(building)]
    endmethod
 
    public static method IsBuildingCurrentlyConstructed takes unit building returns boolean
        return isConstructing[GetUnitId(building)]
    endmethod

endstruct
endlibrary