library OrderMatrix /*

    */ requires /*

         ---------------------------
        |   Legend:                 |
        |    - #     -> Requirement |
        |    - #?    -> Optional    |
         ---------------------------


        --------------------------------------
        */ Table                            /*
        --------------------------------------   
            -> Bribe
         
            link: https://www.hiveworkshop.com/threads/snippet-new-table.188084/#post-1835068
     
        --------------------------------------
        */  UnitDex                         /*
        --------------------------------------
            -> TriggerHappy
         
            link: https://www.hiveworkshop.com/threads/system-unitdex-unit-indexer.248209/
     
        --------------------------------------
        */ optional RegisterPlayerUnitEvent /*
        --------------------------------------
            # RegisterNativeEvent
         
            -> Bannar
         
            link: https://www.hiveworkshop.com/threads/snippet-registerevent-pack.250266/
                     
        Table                       -> Simplifies storage by a mile.
        UnitDex                     -> Allows the system to use GetUnitId instead of GetHandleId
        RegisterPlayerUnitEvent     -> Not really required, but helpful anyway.
     
         -----------------------
        |   OrderMatrix         |
        |   - MyPad             |
        |                       |
        |   Version: 1.4.4      |
         -----------------------
       
        -----------------------------------------------------------
        Description:
     
            OrderMatrix is a library that automatically
            maps out the relevant information of issued orders.
         
            It also stores the previous 5 orders of each unit
            for convenience, although the number is configurable.
        -----------------------------------------------------------
     
        -----------------------------------------------------------
        struct StoreOrders extends array
     
        Members:
         
             -------------------------------------------------------
            |
            |   @configurable static constant integer MAX_ORDERS
            |       (private)
            |
            |       The maximum amount of previous orders to be
            |       stored. You can change this to any positive
            |       value.
            |
            |   integer relevantNode (private)
            |       The node in question when calling
            |       OrderMatrix[unit].past[index].
            |
            |       Made an instance member so as not to cause any
            |       conflicts.
            |
            |   integer lastIndex (private)
            |       The previously stored member of the operator
            |       [].
            |
            |       Made an instance member so as not to cause any
            |       conflicts.
            |
            |   TableArray dataArray (private)
            |       The main storage of data. Note that this is
            |       a per-unit instance.
            |
             -------------------------------------------------------
     
        Methods:
         
             -------------------------------------------------------
            |
            |   method operator [integer index] -> thistype
            |       Stores the relevantNode and lastIndex
            |       and assigns a new value to relevantNode
            |       if the lastIndex is different from the
            |       previous value
            |
            |   method getTargetX() -> real
            |       -> the x-coordinate of past issued order.
            |       The depth of past order depends on the parameter
            |       of the operator [].
            |
            |   method getTargetY() -> real
            |       -> the y-coordinate of past issued order.
            |       The depth of past order depends on the parameter
            |       of the operator [].
            |
            |   method getTarget() -> widget
            |       -> the past targeted widget.
            |       (operator [] dependent)
            |
            |   method getTargetUnit() -> unit
            |       -> the past targeted unit.
            |       (operator [] dependent)
            |
            |   method getTargetDest() -> destructable
            |       -> the past targeted destructable.
            |       (operator [] dependent)
            |
            |   method getTargetItem() -> item
            |       -> the past targeted item.
            |       (operator [] dependent)
            |
            |   method getOrderId() -> integer
            |       -> the past order..
            |       (operator [] dependent)
            |
            |   method getOrderCount() -> integer
            |       -> the amount of orders issued.
            |        Maximum is MAX_ORDERS
            |
            |   method getOrderType() -> integer
            |       -> the order type of the past order
            |       (Either Immediate, Target or Point)
            |       (operator [] dependent)
            |
             -------------------------------------------------------
         
     
        struct OrderMatrix extends array
     
        Members:
     
             -------------------------------------------------------     
            |
            |   static constant integer INFO_SIZE (private)
            |       The amount of space used by the next member.
            |
            |   static TableArray orderData (private)
            |       Stores relevant information, from the x and y
            |       to the target type.
            |
            |   static unit lastUnit (private)
            |       The unit that was passed in the operator []
            |
             -------------------------------------------------------
             
        Methods:
         
             -------------------------------------------------------
            |
            |   static method operator [] (unit u) -> OrderMatrix
            |       Returns the unit id of the unit typecasted
            |       to OrderMatrix.
            |
            |   method operator past() -> StoreOrders
            |       Returns the same instance but typecasted.
            |
            |   method operator [] (integer index) -> StoreOrders
            |       Another way of writing OrderMatrix[unit].past
            |
            |   method getTargetX() -> real
            |       -> the x-coordinate of last issued order.
            |       This may not work with auto-acquire events.
            |
            |   method getTargetY() -> real
            |       -> the y-coordinate of last issued order.
            |       This may not work with auto-acquire events.
            |
            |   method getTarget() -> widget
            |       -> the targeted widget.
            |
            |   method getTargetUnit() -> unit
            |       -> the targeted unit.
            |
            |   method getTargetDest() -> destructable
            |       -> the targeted destructable.
            |
            |   method getTargetItem() -> item
            |       -> the targeted item.
            |
            |   method getOrderId() -> integer
            |       -> the issued order id.
            |
            |   method getOrderType() -> integer
            |       -> the order type of the issued id.
            |
             -------------------------------------------------------
         
        -----------------------------------------------------------
     
        -----------------------------------------------------------
        How to use:
     
        Simply use the struct as follows:
     
            call OrderMatrix[unit].getTargetX()
            call OrderMatrix[unit].getTargetY()
         
        NOTE:
     
            (For the above functions)
         
            It is recommended not to use this within an Issued
            order event, since the current order could cause it
            to break.
         
        Or
     
            call OrderMatrix[unit].past[int].getTargetX()
     
        -----------------------------------------------------------
     
        -----------------------------------------------------------
        Credits:
     
            PurgeandFire for the typecast method for hashtables
                         (Now rewritten to no longer use such an
                          approach, but give credit anyway)
                         
            TriggerHappy for suggesting the expansion of the resource
            Jampion      for pointing out weird bugs (implicit)
                         that caused script-breaking behavior.
                       
        -----------------------------------------------------------
    */

globals
    private constant boolean ALLOW_DEBUGGING    = false
endglobals

private keyword InitModule

private struct StoreOrders extends array
    //  CONFIGURABLES

    //  Configure this amount if you want to store more than five orders.
    private static constant integer MAX_ORDERS  = 8

    //  END CONFIGURABLES
    private static constant integer MAX_SIZE    = 9

    readonly integer lastIndex       
    private integer relevantNode
    private boolean isUpdated

    readonly TableArray dataArray

    //  The following methods are only for readability in the upcoming private methods...
    private method getNext takes integer index returns integer
        return this.dataArray[2].integer[index]
    endmethod

    private method getPrev takes integer index returns integer
        return this.dataArray[3].integer[index]
    endmethod

    private method setNext takes integer index, integer value returns nothing
        set this.dataArray[2].integer[index] = value
    endmethod

    private method setPrev takes integer index, integer value returns nothing
        set this.dataArray[3].integer[index] = value
    endmethod

    private method popOrder takes nothing returns nothing
        local integer lastNode = this.dataArray[3].integer[0]
     
        call this.dataArray[6].unit.remove(lastNode + thistype.MAX_ORDERS)
        call this.dataArray[6].destructable.remove(lastNode + thistype.MAX_ORDERS*2)
        call this.dataArray[6].item.remove(lastNode + thistype.MAX_ORDERS*3)
       
        call this.setNext(this.getPrev(lastNode), this.getNext(lastNode))
        call this.setPrev(this.getNext(lastNode), this.getPrev(lastNode))
     
        //  Deallocate the instance ...
        set this.dataArray[1].integer[lastNode]   = this.dataArray[1].integer[0]
        set this.dataArray[1].integer[0]          = lastNode
       
        //  Decrement the size
        set this.dataArray[1].integer[-1]         = this.dataArray[1].integer[-1] - 1
        set this.dataArray[1].integer[-2]         = ModuloInteger(this.dataArray[1].integer[-2], thistype.MAX_ORDERS) + 1
    endmethod

    method operator [] takes integer index returns thistype
    static if ALLOW_DEBUGGING then
        debug call BJDebugMsg("Index to look at: " + I2S(index) + "\n")  // - This got leaked into release :P
       
        debug if index <= 0 or index >= thistype.MAX_ORDERS then
            debug call BJDebugMsg("thistype.op_getindex: Invalid index parameter.")
            debug call BJDebugMsg("thistype.op_getindex: Index provided: " + I2S(index) + "\n" /*
                            */  + "    -> Maximum orders stored by the system: " + I2S(thistype.MAX_ORDERS))
        debug endif
     
        debug if index > this.dataArray[1].integer[-1] then
            debug if index < thistype.MAX_ORDERS then
                debug call BJDebugMsg("thistype.op_getindex: Attempted to go beyond the order scope of the unit.")
                debug call BJDebugMsg("thistype.op_getindex: Unit hasn't been ordered around much yet.")
            debug call BJDebugMsg("thistype.op_getindex: Index provided: " + I2S(index) + "\n" /*
                    */  + "    -> Current amount of orders stored: " + I2S(this.dataArray[1].integer[-1]))

            debug endif
        debug endif
    endif
         
        //  A simple list behavior allows for potentially O(1) search.
        if (this.lastIndex != index) or (this.isUpdated) then
            set this.isUpdated = false
            set this.lastIndex = index

            set index = this.dataArray[1].integer[-1] + 1 - IMaxBJ(IMinBJ(index, this.dataArray[1].integer[-1]), 1)
            if this.dataArray[1].integer[-2] != 0 then
                set index = ModuloInteger(ModuloInteger(index, thistype.MAX_ORDERS) + this.dataArray[1].integer[-2], thistype.MAX_ORDERS)
                if index == 0 then
                    set index = thistype.MAX_ORDERS
                endif
            endif
     
            set this.relevantNode = index
        endif

    static if ALLOW_DEBUGGING then
        debug call BJDebugMsg("Index result: " + I2S(this.relevantNode))
    endif
   
        return this
    endmethod

    //  Protected method ... don't use outside of the map script...
    method store takes real x, real y, widget targ, integer lastOrder, integer lastOrderType, unit u, destructable d, item i returns nothing
        local integer index
     
        if this.dataArray == 0 then
            set this.dataArray = TableArray[thistype.MAX_SIZE]
        endif
         
        //  Pop the oldest order off.
        if this.dataArray[1].integer[-1] >= MAX_ORDERS then
            call this.popOrder()
        endif
     
        set index = this.dataArray[1].integer[0]
       
        if this.dataArray[1].integer[index] == 0 then
            set index = index + 1
            set this.dataArray[1].integer[0] = index
        else
            set this.dataArray[1].integer[0] = this.dataArray[1].integer[index]
            set this.dataArray[1].integer[index] = 0
        endif
     
        set this.dataArray[1].integer[-1] = this.dataArray[1].integer[-1] + 1
       
        call this.setNext(index, this.getNext(0))
        call this.setPrev(index, 0)
        call this.setNext(this.getPrev(index), index)
        call this.setPrev(this.getNext(index), index)
     
        //  Store the data...
        set this.dataArray[4].real[index]       = x
        set this.dataArray[5].real[index]       = y
        set this.dataArray[6].widget[index]     = targ
        set this.dataArray[7].integer[index]    = lastOrder
        set this.dataArray[8].integer[index]    = lastOrderType

        set this.dataArray[6].unit[index + thistype.MAX_ORDERS]           = u
        set this.dataArray[6].destructable[index + thistype.MAX_ORDERS*2] = d
        set this.dataArray[6].item[index + thistype.MAX_ORDERS*3]         = i

        set this.isUpdated                      = true
    endmethod
   
    method getTargetX takes nothing returns real
        return this.dataArray[4].real[this.relevantNode]
    endmethod

    method getTargetY takes nothing returns real
        return this.dataArray[5].real[this.relevantNode]
    endmethod

    method getTarget takes nothing returns widget
        return this.dataArray[6].widget[this.relevantNode]
    endmethod

    method getTargetUnit takes nothing returns unit
        return this.dataArray[6].unit[this.relevantNode + thistype.MAX_ORDERS]
    endmethod

    method getTargetDest takes nothing returns destructable
        return this.dataArray[6].destructable[this.relevantNode + thistype.MAX_ORDERS*2]
    endmethod

    method getTargetItem takes nothing returns item
        return this.dataArray[6].item[this.relevantNode + thistype.MAX_ORDERS*3]
    endmethod

    method getOrderId takes nothing returns integer
        return this.dataArray[7].integer[relevantNode]
    endmethod

    method getOrderType takes nothing returns integer
        return this.dataArray[8].integer[relevantNode]
    endmethod

    method getOrderCount takes nothing returns integer
        return this.dataArray[1].integer[-1]
    endmethod
   
    private static method onEnter takes nothing returns nothing
        local thistype instance = GetIndexedUnitId()
     
        if instance.dataArray == 0 then
            set instance.dataArray  = TableArray[thistype.MAX_SIZE]
         
            call instance.setNext(0, 0)
            call instance.setPrev(0, 0)
        endif
    endmethod

    private static method onExit takes nothing returns nothing
        local thistype instance = GetIndexedUnitId()
     
        if instance.dataArray != 0 then
            call instance.dataArray.destroy()
         
            set instance.dataArray = 0
            set instance.isUpdated = false
        endif
    endmethod

    static method init takes nothing returns nothing
        call RegisterUnitIndexEvent(Condition(function thistype.onEnter), EVENT_UNIT_INDEX)
        call RegisterUnitIndexEvent(Condition(function thistype.onExit), EVENT_UNIT_DEINDEX)
    endmethod

    implement InitModule
endstruct

struct OrderMatrix extends array
    private static constant integer INFO_SIZE           = 5

    readonly static constant integer IMMEDIATE_ORDER    = 1
    readonly static constant integer POINT_ORDER        = 2
    readonly static constant integer TARGET_ORDER       = 3

    private static TableArray orderData                 = 0
    private static unit       lastUnit                  = null

    static method operator [] takes unit u returns thistype
        set lastUnit = u
        return thistype(GetUnitId(u))
    endmethod

    method operator past takes nothing returns StoreOrders
        return StoreOrders(this)
    endmethod

    method operator [] takes integer index returns StoreOrders
        return this.past[index]
    endmethod

    /*
   
        Deprecated: No more functionality!
       
    //  Clears out the current data of issued order.
    private method clear takes nothing returns nothing
        if UnitDex.Initialized then
            call this.past.store(orderData[1].real[this], orderData[2].real[this], orderData[3].widget[this]/*
                             */, orderData[INFO_SIZE].integer[this], orderData[4].integer[this] /*
                             */, orderData[3].unit[this + (JASS_MAX_ARRAY_SIZE - 1)]            /*
                             */, orderData[3].destructable[this + (JASS_MAX_ARRAY_SIZE - 1)*2]  /*
                             */, orderData[3].item[this + (JASS_MAX_ARRAY_SIZE - 1)*3])
        endif
     
        set orderData[1].real[this]    = 0.
        set orderData[2].real[this]    = 0.
        set orderData[3].widget[this]  = null
        set orderData[4].integer[this] = 0
     
        set orderData[INFO_SIZE].integer[this] = GetUnitCurrentOrder(lastUnit)
    endmethod

    //  Checks if the current order is the same as the issued order.
    private method assertOrder takes nothing returns boolean
        return GetUnitCurrentOrder(lastUnit) == orderData[INFO_SIZE].integer[this]
    endmethod
    */
     
    method getOrderId takes nothing returns integer
        return orderData[INFO_SIZE].integer[this]
    endmethod

    method getOrderType takes nothing returns integer
        return orderData[4].integer[this]
    endmethod

    method getTargetX takes nothing returns real
        return orderData[1].real[this]
    endmethod

    method getTargetY takes nothing returns real
        return orderData[2].real[this]
    endmethod

    method getTarget takes nothing returns widget
        return thistype.orderData[3].widget[this]
    endmethod

    method getTargetUnit takes nothing returns unit
        return thistype.orderData[3].unit[this + (JASS_MAX_ARRAY_SIZE - 1)]
    endmethod

    method getTargetDest takes nothing returns destructable
        return thistype.orderData[3].destructable[this + (JASS_MAX_ARRAY_SIZE - 1)*2]
    endmethod

    method getTargetItem takes nothing returns item
        return thistype.orderData[3].item[this + (JASS_MAX_ARRAY_SIZE - 1)*3]
    endmethod

    private static method onIssueOrder takes nothing returns nothing
        local thistype inst = thistype[GetTriggerUnit()]
        local integer wId
        local widget w
     
        if UnitDex.Initialized then
            set w       = GetOrderTarget()
            set wId     = GetHandleId(w)
         
            call inst.past.store(orderData[1].real[inst], orderData[2].real[inst], orderData[3].widget[inst]/*
                             */, orderData[INFO_SIZE].integer[inst], orderData[4].integer[inst] /*
                             */, orderData[3].unit[inst + (JASS_MAX_ARRAY_SIZE - 1)]            /*
                             */, orderData[3].destructable[inst + (JASS_MAX_ARRAY_SIZE - 1)*2]  /*
                             */, orderData[3].item[inst + (JASS_MAX_ARRAY_SIZE - 1)*3])

            set orderData[INFO_SIZE].integer[inst] = GetIssuedOrderId()
         
            set orderData[1].real[inst] = GetOrderPointX()
            set orderData[2].real[inst] = GetOrderPointY()
         
            set orderData[3].widget[inst]  = w
         
            call orderData[3].unit.remove(inst + (JASS_MAX_ARRAY_SIZE - 1))
            call orderData[3].destructable.remove(inst + (JASS_MAX_ARRAY_SIZE - 1)*2)
            call orderData[3].item.remove(inst + (JASS_MAX_ARRAY_SIZE - 1)*3)
         
            if GetTriggerEventId() == EVENT_PLAYER_UNIT_ISSUED_ORDER then
                set orderData[4].integer[inst] = thistype.IMMEDIATE_ORDER
             
            elseif GetTriggerEventId() == EVENT_PLAYER_UNIT_ISSUED_POINT_ORDER then
                set orderData[4].integer[inst] = thistype.POINT_ORDER
             
            elseif GetTriggerEventId() == EVENT_PLAYER_UNIT_ISSUED_TARGET_ORDER then
                set orderData[4].integer[inst] = thistype.TARGET_ORDER
             
                if wId == GetHandleId(GetOrderTargetUnit()) then
                    set orderData[3].unit[inst + (JASS_MAX_ARRAY_SIZE - 1)]               = GetOrderTargetUnit()
                elseif wId == GetHandleId(GetOrderTargetDestructable()) then
                    set orderData[3].destructable[inst + (JASS_MAX_ARRAY_SIZE - 1)*2]     = GetOrderTargetDestructable()
                elseif wId == GetHandleId(GetOrderTargetItem()) then
                    set orderData[3].item[inst + (JASS_MAX_ARRAY_SIZE - 1)*3]             = GetOrderTargetItem()
                endif
            endif
         
            set w = null
        endif
    endmethod

    private static method initVar takes nothing returns nothing
        set orderData = TableArray[INFO_SIZE + 1]
    endmethod

    private static method initTrig takes nothing returns nothing
     
    static if LIBRARY_RegisterPlayerUnitEvent then
        call RegisterAnyPlayerUnitEvent(EVENT_PLAYER_UNIT_ISSUED_ORDER, function thistype.onIssueOrder)
        call RegisterAnyPlayerUnitEvent(EVENT_PLAYER_UNIT_ISSUED_POINT_ORDER, function thistype.onIssueOrder)
        call RegisterAnyPlayerUnitEvent(EVENT_PLAYER_UNIT_ISSUED_TARGET_ORDER, function thistype.onIssueOrder)
    else
        local trigger t = CreateTrigger()
     
        call TriggerRegisterAnyUnitEventBJ(t, EVENT_PLAYER_UNIT_ISSUED_ORDER)
        call TriggerRegisterAnyUnitEventBJ(t, EVENT_PLAYER_UNIT_ISSUED_POINT_ORDER)
        call TriggerRegisterAnyUnitEventBJ(t, EVENT_PLAYER_UNIT_ISSUED_TARGET_ORDER)
     
        call TriggerAddCondition(t, Condition(function thistype.onIssueOrder))
     
        set t = null
    endif
    endmethod

    private static method init takes nothing returns nothing
        call initVar()
        call initTrig()     
    endmethod

    implement InitModule
endstruct

private module InitModule
    private static method onInit takes nothing returns nothing
        call init()   
    endmethod
endmodule

endlibrary