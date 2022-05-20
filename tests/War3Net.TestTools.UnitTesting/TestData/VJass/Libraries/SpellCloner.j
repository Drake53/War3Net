library SpellCloner /* v2.2.0 https://www.hiveworkshop.com/threads/324157/


    */uses /*

    */SpellEvent            /*  https://www.hiveworkshop.com/threads/301895/
    */Table                 /*  https://www.hiveworkshop.com/threads/188084/


    *///! novjass

    /*
        CREDITS:
            - AGD (Author)
            - JAKEZINC (Feedbacks and suggestions, which helped bring the system into its current form)
    */
    |=====|
    | API |
    |=====|

        readonly static SpellCloner.configuredInstance/*
                - Use this variable inside the configuration function to refer to the spell
                  instance being configured


      */module SpellClonerHeader/*
            - Implement this module at the top of your spell struct

          */static method hasActivationAbility takes integer abilId returns boolean/*
          */static method hasConfiguration takes integer configStructId returns boolean/*
                - Only call this from inside a spell event handler (generic or specific)
                - Could be useful for especially in generic handlers to see if the casted
                  ability activates a certain type of spell, as cloned spell can have many
                  activation abilities

          */method initSpellConfiguration takes integer abilId returns integer/*
                - Call this method at the top of you onSpellStart() method to initialize the
                  correct local configuration of your spell instance based on the activation
                  ability id
                - Returns the struct type id of the struct containing the configuration

          */method loadSpellConfiguration takes integer configStructId returns nothing/*
                - Call this method with the value returned by initSpellConfiguration() as the
                  parameter
                - Like initSpellConfiguration(), loads the correct local configuration of the
                  spell, but based on the typeid of the configuration struct


      */module SpellClonerFooter/*
            - Implement this module at the bottom of your spell struct, below your SpellEvent implementation

          */static method create takes integer configStructId, integer abilId, integer spellEventType, code configurationFunc returns thistype/*
                - Creates a new local configuration instance for the spell (Return value is obsolete)


      */module SpellCloner/*
            - A supplement to using both SpellClonerHeader and SpellClonerFotter. Implement this
              module at the bottom of your spell struct, no need to implement the SpellEvent module

          */interface method    onClonedSpellStart      takes nothing   returns thistype/*
          */interface method    onClonedSpellPeriodic   takes nothing   returns boolean/*
          */interface method    onClonedSpellEnd        takes nothing   returns nothing/*
                - Supplement to the onSpellStart(), onSpellPeriodic(), and onSpellEnd() methods from
                  SpellEvent module
                - All these interface methods follow the same rules as their SpellEvent interface methods
                  counterpart
                - You no longer need to call this.initSpellConfiguration(ABIL_ID) on onClonedSpellStart()
                  to run your configuration function, this is already done internally by the system.

          */static method hasActivationAbility takes integer abilId returns boolean/*
          */static method hasConfiguration takes integer configStructId returns boolean/*
                - Already defined above (see SpellClonerHeader module)

          */static method create takes integer configStructId, integer abilId, integer spellEventType, code configurationFunc returns thistype/*
                - Creates a new local configuration instance for the spell (Return value is obsolete)


    *///! endnovjass

    globals
        private trigger evaluator = CreateTrigger()
        private integer array eventIndex
        private integer configuredSpellInstance = 0
        private TableArray table
        private TableArray configStructNode
    endglobals

    private module Init
        private static method onInit takes nothing returns nothing
            set table = TableArray[JASS_MAX_ARRAY_SIZE]
            set configStructNode = TableArray[JASS_MAX_ARRAY_SIZE]
            set eventIndex[EVENT_SPELL_CAST]    = 1
            set eventIndex[EVENT_SPELL_CHANNEL] = 2
            set eventIndex[EVENT_SPELL_EFFECT]  = 3
            set eventIndex[EVENT_SPELL_ENDCAST] = 4
            set eventIndex[EVENT_SPELL_FINISH]  = 5
        endmethod
    endmodule

    struct SpellCloner extends array
        static method operator configuredInstance takes nothing returns thistype
            return configuredSpellInstance
        endmethod
        implement Init
    endstruct

    private struct SpellConfigList extends array
        thistype current
        readonly thistype prev
        readonly thistype next
        readonly integer structId
        readonly boolexpr configExpr

        private static thistype node = 0

        method evaluateExpr takes integer spellInstance returns nothing
            local integer prevInstance = configuredSpellInstance
            set configuredSpellInstance = spellInstance
            call TriggerAddCondition(evaluator, this.configExpr)
            call TriggerEvaluate(evaluator)
            call TriggerClearConditions(evaluator)
            set configuredSpellInstance = prevInstance
        endmethod

        method insert takes integer id, boolexpr expr returns thistype
            local thistype next = this.next
            set node = node + 1
            set node.structId = id
            set node.configExpr = expr
            set node.prev = this
            set node.next = next
            set next.prev = node
            set this.next = node
            return node
        endmethod

        static method create takes nothing returns thistype
            set node = node + 1
            set node.prev = node
            set node.next = node
            set node.current = node
            return node
        endmethod
    endstruct

    public function HasActivationAbility takes integer spellStructId, integer abilId returns boolean
        if GetEventSpellEventType() == 0 then
            return table[spellStructId*5 + 1].has(-abilId)
        endif
        return table[spellStructId*5 + eventIndex[GetEventSpellEventType()]].has(abilId)
    endfunction
    public function HasConfiguration takes integer spellStructId, integer configStructId returns boolean
        if GetEventSpellEventType() == 0 then
            return configStructNode[spellStructId*5 + 1].has(-configStructId)
        endif
        return configStructNode[spellStructId*5 + eventIndex[GetEventSpellEventType()]].has(configStructId)
    endfunction

    public function InitSpellConfiguration takes integer spellStructId, integer spellInstance, integer abilId returns integer
        local integer configStructId
        local SpellConfigList configList
        if GetEventSpellEventType() == 0 then
            set configList = table[spellStructId*5 + 1][-abilId]
        else
            set configList = table[spellStructId*5 + eventIndex[GetEventSpellEventType()]][abilId]
        endif
        set configList.current = configList.current.next
        set configStructId = configList.current.structId
        call configList.current.evaluateExpr(spellInstance)
        if configList.current.next == configList then
            set configList.current = configList
        endif
        return configStructId
    endfunction

    public function LoadSpellConfiguration takes integer spellStructId, integer spellInstance, integer configStructId returns nothing
        if GetEventSpellEventType() == 0 then
            call SpellConfigList(configStructNode[spellStructId*5 + 1][-configStructId]).evaluateExpr(spellInstance)
        else
            call SpellConfigList(configStructNode[spellStructId*5 + eventIndex[GetEventSpellEventType()]][configStructId]).evaluateExpr(spellInstance)
        endif
    endfunction

    public function CloneSpell takes integer spellStructId, integer configStructId, integer abilId, integer eventType, code configFunc returns nothing
        local SpellConfigList configList
        local integer eventId = 0x10
        local integer key
        if eventType == 0 then
            set key = spellStructId*5 + 1
            if configStructNode[key][-configStructId] == 0 then
                set configList = table[key][-abilId]
                if configList == 0 then
                    set configList = SpellConfigList.create()
                    set table[key][-abilId] = configList
                endif
                set configStructNode[key][-configStructId] = configList.prev.insert(configStructId, Filter(configFunc))
            endif
        else
            loop
                exitwhen eventId == 0
                if eventType >= eventId then
                    set eventType = eventType - eventId
                    set key = spellStructId*5 + eventIndex[eventId]
                    if configStructNode[key][configStructId] == 0 then
                        set configList = table[key][abilId]
                        if configList == 0 then
                            set configList = SpellConfigList.create()
                            set table[key][abilId] = configList
                        endif
                        set configStructNode[key][configStructId] = configList.prev.insert(configStructId, Filter(configFunc))
                    endif
                endif
                set eventId = eventId/2
            endloop
        endif
    endfunction

    private module SpellClonerCommonHeader
        static constant integer SPELL_ABILITY_ID = 0
        static constant integer SPELL_EVENT_TYPE = 0

        static method hasActivationAbility takes integer abilId returns boolean
            return HasActivationAbility(thistype.typeid, abilId)
        endmethod
        static method hasConfiguration takes integer configStructId returns boolean
            return HasConfiguration(thistype.typeid, configStructId)
        endmethod
    endmodule

    module SpellClonerHeader
        implement SpellClonerCommonHeader
        method initSpellConfiguration takes integer abilId returns integer
            return InitSpellConfiguration(thistype.typeid, this, abilId)
        endmethod
        method loadSpellConfiguration takes integer configStructId returns nothing
            call LoadSpellConfiguration(thistype.typeid, this, configStructId)
        endmethod
    endmodule

    module SpellClonerFooter
        static method create takes integer configStructId, integer abilId, integer spellEventType, code configurationFunc returns thistype
            call CloneSpell(thistype.typeid, configStructId, abilId, spellEventType, configurationFunc)
            call registerSpellEvent(abilId, spellEventType)
            return 0
        endmethod
    endmodule

    module SpellCloner
        implement SpellClonerCommonHeader
        method onSpellStart takes nothing returns thistype
            local integer configStructId = InitSpellConfiguration(thistype.typeid, this, GetEventSpellAbilityId())
            local thistype node = this.onClonedSpellStart()
            if node > 0 then
                if node != this then
                    call SpellConfigList(configStructNode[thistype.typeid][configStructId]).evaluateExpr(node)
                endif
                return node
            endif
            return 0
        endmethod
        method onSpellPeriodic takes nothing returns boolean
            return this.onClonedSpellPeriodic()
        endmethod
        method onSpellEnd takes nothing returns nothing
            call this.onClonedSpellEnd()
        endmethod
        implement SpellEvent
        implement SpellClonerFooter
    endmodule


endlibrary