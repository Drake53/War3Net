library GenericUnitEvent /*
    --------------
    */ requires /*
    --------------
   
        --------------
        */ UnitDex  /*
        --------------
            #? GroupUtils
            #? WorldBounds
       
            By TriggerHappy:
            link: https://www.hiveworkshop.com/threads/system-unitdex-unit-indexer.248209/
           
        --------------
        */ Table    /*
        --------------
            By Bribe:
            link: https://www.hiveworkshop.com/threads/snippet-new-table.188084/
           
        --------------
        */ ListT    /*
        --------------
            ## Table
            #  Alloc
           
            By Bannar:
            link: https://www.hiveworkshop.com/threads/containers-list-t.249011/page-2#post-3271599
           
         ----------------------------------------------------
        |                                                    |
        |       GenericUnitEvent                             |
        |           - MyPad                                  |
        |                                                    |
         ----------------------------------------------------
        
         -------------------------------------------------------------------------------
        |
        |   Restrictions:
        |
        |       - Units cannot be manually deindexed, as the system treats such an event
        |       as a unit removal event.
        |
        |-------------------------------------------------------------------------------
        |
        |   Description:
        |  
        |       - A library which makes unitevent exclusive events easy to register and
        |       listen to. Essentially extends unitevent to pseudo-playerunitevent.
        |
        |       - Makes damage detection a breeze, as well as target acquisition
        |       and target in range events.
        |
        |
        |--------------------------------------------------------------------------------
        |
        |   API:
        |
        |   struct GenericUnitEvent:
        |       static method listen(unitevent whichEvent) -> StubUnitEvent
        |           - Enables the system to listen to that specified unitevent.
        |
        |       static method registerUnit(unit whichUnit) -> boolean
        |           - Registers a unit to the list of listened unitevents.
        |             This is automatically called on unit creation
        |             if REGISTER_ON_STARTUP is set to true. (Default)
        |
        |   struct StubUnitEvent:
        |       method addHandler(code callback) -> triggercondition
        |           - Adds a callback function to be invoked upon the execution
        |             of a certain event.
        |
        |       method removeHandler(triggercondition cond)
        |           - Removes a callback function from a certain event.
        |
        |   Functions:
        |       function RegisterUnitEvent(unitevent whichEvent, code callback) -> triggercondition
        |           - Executes a condensed version of the code, without having to deal too much
        |             with structs.
        |
        |       function RegisterAnyUnitEvent(unitevent uv, code c) -> triggercondition
        |           - A deprecated function that invokes RegisterUnitEvent.
        |
        |       function RegisterUnitEventById(integer id, code callback) -> trggercondition
        |           - Internally calls RegisterUnitEvent
        |
        |       function GetUnitEventId() -> unitevent
        |           - Returns the triggering event in parallel with GetTriggerEventId()
        |             Can also persist where GetTriggerEventId might fail.
        |
        |--------------------------------------------------------------------------------
        |
        |   Changelogs:
        |
        |       v.1.1 - Rewritten the entire library.
        |             - Removed SetUnitBucketSize.
        |
        |       v.1.0 - Release
        |
        |---------------------------------------------------------------------------------
        |
        |   Programmer notes:
        |
        |       In the previous version, the following would compile:
        |           local integer i
        |           call GenericUnitEvent(i).addHandler(code callback)
        |
        |       Now, the lines above would no longer compile. Instead, try this:
        |
        |           call GenericUnitEvent.listen(yourUnitEvent).addHandler(code callback)
        |
         -----------------------------------------------------------------------------------
        
         -----------------------
        |
        |   Credits:
        |
        |       - AGD for the inputs on this system.
        |       - Nestharus for the ideal management
        |           of the linked list structure
        |           (Which the rewrite was guided on)
        |       - Bribe for Table (The HIVE one)
        |       - Bannar for ListT
        |
         -----------------------
    */

native UnitAlive takes unit id returns Boolean

globals
    private constant boolean REGISTER_ON_STARTUP    = true
    private constant boolean ADVANCED_DEBUG         = false
    private constant integer MAX_BUCKET_SIZE        = 5
    private constant integer REFRESH_AMOUNT         = 3
endglobals

private function DuplicateList takes IntegerList whichList returns IntegerList
    local IntegerList newList   = IntegerList.create()
    local IntegerListItem iter  = whichList.first
    loop
        exitwhen iter == 0
        call newList.push(iter.data)
        set iter = iter.next
    endloop
    return newList
endfunction

private module Initializer
    private static method onInit takes nothing returns nothing
        call thistype.init()
    endmethod
endmodule

private struct StubUnitEvent extends array
    implement Alloc
   
    private static Table eventMap                   = 0
   
    readonly unitevent event
    readonly trigger handlerTrig
   
    method removeHandler takes triggercondition cond returns nothing
        call TriggerRemoveCondition(this.handlerTrig, cond)
    endmethod
   
    method addHandler takes code callback returns triggercondition
        return TriggerAddCondition(this.handlerTrig, Condition(callback))
    endmethod
   
    static method peek takes eventid whichId returns thistype
        return eventMap[GetHandleId(whichId)]
    endmethod
   
    static method request takes unitevent whichEvent returns thistype
        local thistype this         = eventMap[GetHandleId(whichEvent)]
        if this == 0 then
            set this                = .allocate()
            set this.event          = whichEvent
            set this.handlerTrig    = CreateTrigger()
            set eventMap[GetHandleId(whichEvent)] = this
        endif
        return this
    endmethod
   
    private static method init takes nothing returns nothing
        set thistype.eventMap       = Table.create()
    endmethod
   
    implement Initializer
endstruct

struct GenericUnitEvent extends array
    implement Alloc
   
    private static code  registerPointer    = null
   
    private static group swap           = null
    private static group container      = null
    private static group tempRegContain = null
   
    private static Table stubMap        = 0
    private static IntegerList stubList = 0
   
    private static IntegerList array reqList
    private static integer removeCount  = 0
   
    //  For GenericUnitEvent members
    private trigger detector
    private group   groupHolder
    private integer unitsRegistered
    private integer unitsRemoved
   
    private IntegerList reqListPointer
    private IntegerListItem reqListItem
   
    //  For UnitDex members only
    private static boolean array existsForIndex
    private static thistype array keyGroup
   
    //  Iterate over global group
    private static StubUnitEvent tempStub   = 0
   
static if ADVANCED_DEBUG then
    debug private static boolean stackTrace     = false
    debug private static thistype stackObject   = 0
endif
    readonly static unitevent execUnitEvent     = null
    
    private static method onEventExecution takes nothing returns nothing
        local StubUnitEvent object  = StubUnitEvent.peek(GetTriggerEventId())
        local unitevent lastEv
        if object != 0 then
            set lastEv          = .execUnitEvent
            set .execUnitEvent  = object.event
            
            if IsTriggerEnabled(object.handlerTrig) then
                call TriggerEvaluate(object.handlerTrig)
            endif
            
            set .execUnitEvent  = lastEv
            set lastEv          = null
        endif
    endmethod
   
    private method destruct takes nothing returns nothing
        local unit u
        local integer id
       
        call DestroyTrigger(this.detector)
        loop
            set u   = FirstOfGroup(this.groupHolder)
            set id  = GetUnitId(u)
           
            exitwhen u == null
           
            set keyGroup[id]        = 0
            set existsForIndex[id]  = false
           
            call GroupRemoveUnit(.container, u)
            call GroupRemoveUnit(this.groupHolder, u)
            call GroupAddUnit(.tempRegContain, u)
        endloop
        call DestroyGroup(this.groupHolder)
        call this.reqListPointer.erase(this.reqListItem)
       
    static if ADVANCED_DEBUG then
        debug set .stackTrace   = true
        debug set .stackObject  = this
    endif
   
        call ForForce(bj_FORCE_PLAYER[0], .registerPointer)
   
    static if ADVANCED_DEBUG then
        debug set .stackTrace   = false
    endif
   
        set .removeCount         = .removeCount - this.unitsRemoved
               
        set this.unitsRegistered = 0
        set this.unitsRemoved    = 0
        set this.reqListPointer  = 0
        set this.reqListItem     = 0
        set this.groupHolder     = null
        set this.detector        = null
       
        call this.deallocate()
    endmethod
   
    private static method construct takes nothing returns thistype
        local thistype this     = .allocate()
        set this.detector       = CreateTrigger()
        set this.groupHolder    = CreateGroup()
        call TriggerAddCondition(this.detector, Condition(function thistype.onEventExecution))
        return this
    endmethod
   
    private static method onNewStubUnitEvent takes StubUnitEvent temp returns nothing
        local unit u            = FirstOfGroup(.container)
        local group tempSwap    = .swap
        local integer id        = GetUnitId(u)
        loop
            exitwhen u == null
           
            call GroupRemoveUnit(.container, u)
            call GroupAddUnit(.swap, u)
           
            call TriggerRegisterUnitEvent(keyGroup[id].detector, u, temp.event)
           
            set u   = FirstOfGroup(.container)
            set id  = GetUnitId(u)
        endloop
       
        set .swap           = .container
        set .container      = tempSwap
        set tempSwap        = null
        set u               = null
    endmethod
   
    private static method onListenCallback takes nothing returns nothing
        call thistype.onNewStubUnitEvent(.tempStub)
    endmethod
   
    static method registerUnit takes unit whichUnit returns boolean
        local integer id = GetUnitId(whichUnit)
        local thistype this
        local IntegerListItem iter
       
        if (id == 0) or existsForIndex[id] then
            return false
        endif
        // Find out if there are any instances that need to be filled up.
        if .reqList[1].size() != 0 then
            set this = .reqList[1].first.data
            static if ADVANCED_DEBUG then
                debug if .stackTrace then
                    debug call DisplayTimedTextToPlayer(GetLocalPlayer(), 0, 0, 50000, "GenericUnitEvent::registerUnit -> Bucket retrieval type:: Node reference")
                debug endif
            endif
        else
            set this = thistype.construct()
            static if ADVANCED_DEBUG then
                debug if .stackTrace then
                    debug call DisplayTimedTextToPlayer(GetLocalPlayer(), 0, 0, 50000, "GenericUnitEvent::registerUnit -> Bucket retrieval type:: Node creation")
                debug endif
            endif
           
            call .reqList[0].push(this)
           
            set this.reqListPointer = .reqList[1]
            set this.reqListItem    = .reqList[1].push(this).last
        endif
        static if ADVANCED_DEBUG then
            debug if .stackTrace then
                debug call DisplayTimedTextToPlayer(GetLocalPlayer(), 0, 0, 50000, "GenericUnitEvent::registerUnit -> Bucket instance identified {" + I2S(this) + "}")
            debug endif
        endif
        call GroupAddUnit(.container, whichUnit)
       
        set this.unitsRegistered    = this.unitsRegistered + 1
        call GroupAddUnit(this.groupHolder, whichUnit)
        set existsForIndex[id]      = true
        set keyGroup[id]            = this
       
        //  Register all StubUnitEvents for the unit
        set iter = .stubList.first
        loop
            exitwhen iter == 0
            call TriggerRegisterUnitEvent(this.detector, whichUnit, StubUnitEvent(iter.data).event)
            set iter = iter.next
        endloop
        if this.unitsRegistered >= MAX_BUCKET_SIZE then
            call this.reqListPointer.erase(this.reqListItem)
           
            set this.reqListPointer = .reqList[2]
            set this.reqListItem    = .reqList[2].push(this).last
        endif
        return true
    endmethod
   
    private static method deregisterUnit takes unit whichUnit returns boolean
        local integer id = GetUnitId(whichUnit)
        local thistype this
       
        if (id == 0) or (not existsForIndex[id]) or UnitAlive(whichUnit) then
            return false
        endif
        
        set this = keyGroup[id]
        //  If previously belonging to a full list, move it to list that requests filling up
        if this.reqListPointer == .reqList[2] then
            call .reqList[2].erase(this.reqListItem)
           
            set this.reqListPointer = .reqList[1]
            set this.reqListItem    = .reqList[1].unshift(this).first
        endif
        call GroupRemoveUnit(.container, whichUnit)
       
        call GroupRemoveUnit(this.groupHolder, whichUnit)
        set keyGroup[id]        = 0
        set existsForIndex[id]  = false
       
        set this.unitsRegistered = this.unitsRegistered - 1
        set this.unitsRemoved    = this.unitsRemoved + 1
        set .removeCount         = .removeCount + 1
       
        if (this.unitsRemoved >= REFRESH_AMOUNT) then
            static if ADVANCED_DEBUG then
                debug if .stackTrace then
                    debug call DisplayTimedTextToPlayer(GetLocalPlayer(), 0, 0, 50000, "GenericUnitEvent::deregisterUnit -> calling .destruct() {Attribute: " + I2S(this) + "}")
                debug endif
            endif
            call this.destruct()
            static if ADVANCED_DEBUG then
                debug if .stackTrace then
                    debug call DisplayTimedTextToPlayer(GetLocalPlayer(), 0, 0, 50000, "GenericUnitEvent::deregisterUnit -> .destruct() procedure finished!")
                debug endif
            endif           
        elseif ((.reqList[0].size() > 2) and (.removeCount >= REFRESH_AMOUNT*.reqList[0].size()/2)) then
            set .reqList[3]     = DuplicateList(.reqList[1])
            loop
                exitwhen .reqList[3].first == 0
               
                call thistype(.reqList[3].first.data).destruct()
                call .reqList[3].shift()
            endloop
            call .reqList[3].destroy()
            set .reqList[3]     = 0
        endif
        return true
    endmethod
   
    static method listen takes unitevent whichEvent returns StubUnitEvent
        local StubUnitEvent object  = StubUnitEvent.request(whichEvent)
        if not .stubMap.has(object) then
            set .stubMap[object]     = stubList.push(object).last
            set .tempStub            = object
           
            call ForForce(bj_FORCE_PLAYER[0], function thistype.onListenCallback)
        endif
        return object
    endmethod
    private static method onReRegister takes nothing returns nothing
        local unit u
        local integer id
        loop
            set u   = FirstOfGroup(.tempRegContain)
            set id  = GetUnitId(u)
           
            exitwhen u == null
           
            call thistype.registerUnit(u)
            call GroupRemoveUnit(.tempRegContain, u)
        endloop
    endmethod
   
    private static method onUnitExit takes nothing returns nothing
        call thistype.deregisterUnit(GetIndexedUnit())
    endmethod
static if REGISTER_ON_STARTUP then
    private static method onUnitEnter takes nothing returns nothing
        call thistype.registerUnit(GetIndexedUnit())
    endmethod
endif
   
    private static method initListener takes nothing returns nothing
    static if REGISTER_ON_STARTUP then
        call OnUnitIndex(function thistype.onUnitEnter)
    endif
        call OnUnitDeindex(function thistype.onUnitExit)
    endmethod
    private static method init takes nothing returns nothing
        set thistype.swap               = CreateGroup()
        set thistype.container          = CreateGroup()
        set thistype.tempRegContain     = CreateGroup()
       
        set thistype.registerPointer    = function thistype.onReRegister
        set thistype.stubMap            = Table.create()
        set thistype.stubList           = IntegerList.create()
       
        //  Holds all instances
        set thistype.reqList[0]         = IntegerList.create()
       
        //  Holds instances that are in need of filling up
        set thistype.reqList[1]         = IntegerList.create()
        //  Holds instances that are already filled up.
        set thistype.reqList[2]         = IntegerList.create()
       
        call thistype.initListener()
    endmethod
   
    implement Initializer
endstruct

function RegisterUnitEvent takes unitevent whichEvent, code callback returns triggercondition
    return GenericUnitEvent.listen(whichEvent).addHandler(callback)
endfunction

function RegisterAnyUnitEvent takes unitevent uv, code c returns triggercondition
    debug call BJDebugMsg("GenericUnitEvent::RegisterAnyUnitEvent -> This has been deprecated, please use GenericUnitEvent::RegisterUnitEvent instead.")
    return RegisterUnitEvent(uv, c)
endfunction

function RegisterUnitEventById takes integer id, code callback returns triggercondition
    return RegisterUnitEvent(ConvertUnitEvent(id), callback)
endfunction

function GetUnitEventId takes nothing returns unitevent
    return GenericUnitEvent.execUnitEvent
endfunction

endlibrary