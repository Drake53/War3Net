library Alloc /* v1.1.0 https://www.hiveworkshop.com/threads/324937/


    */uses /*

    */Table                 /*  https://www.hiveworkshop.com/threads/188084/

    */optional ErrorMessage /*  https://github.com/nestharus/JASS/blob/master/jass/Systems/ErrorMessage/main.j


    *///! novjass

    /*
        Written by AGD, based on MyPad's allocation algorithm

            A allocator module using a single global indexed stack. Allocated values are
            within the JASS_MAX_ARRAY_SIZE. No more need to worry about code bloats behind
            the module implementation as it generates the least code possible (6 lines of
            code in non-DEBUG_MODE), nor does it use an initialization function. This system
            also only uses ONE variable (for the whole map) for the hashtable.
    */
    |-----|
    | API |
    |-----|
    /*
      */module GlobalAlloc/*
            - Uses a single stack globally
      */module Alloc/*
            - Uses a unique stack per struct

          */debug readonly boolean allocated/* Is node allocated?

          */static method allocate takes nothing returns thistype/*
          */method deallocate takes nothing returns nothing/*

    *///! endnovjass

    /*===========================================================================*/

    globals
        private key stack
    endglobals

    static if DEBUG_MODE then
        private function AssertError takes boolean condition, string methodName, string structName, integer node, string message returns nothing
            static if LIBRARY_ErrorMessage then
                call ThrowError(condition, SCOPE_PREFIX, methodName, structName, node, message)
            else
                if condition then
                    call BJDebugMsg("[Library: " + SCOPE_PREFIX + "] [Struct: " + structName + "] [Method: " + methodName + "] [Instance: " + I2S(node) + "] : |cffff0000" + message + "|r")
                endif
            endif
        endfunction

        public function IsAllocated takes integer typeId, integer node returns boolean
            return node > 0 and Table(stack)[typeId*JASS_MAX_ARRAY_SIZE + node] == 0
        endfunction
    endif

    public function Allocate takes integer typeId returns integer
        local integer offset = typeId*JASS_MAX_ARRAY_SIZE
        local integer node = Table(stack)[offset]
        local integer stackNext = Table(stack)[offset + node]
        debug call AssertError(typeId < 0, "allocate()", Table(stack).string[-typeId], 0, "Invalid struct ID (" + I2S(typeId) + ")")
        if stackNext == 0 then
            debug call AssertError(node == (JASS_MAX_ARRAY_SIZE - 1), "allocate()", Table(stack).string[-typeId], node, "Overflow")
            set node = node + 1
            set Table(stack)[offset] = node
        else
            set Table(stack)[offset] = stackNext
            set Table(stack)[offset + node] = 0
        endif
        return node
    endfunction
    public function Deallocate takes integer typeId, integer node returns nothing
        local integer offset = typeId*JASS_MAX_ARRAY_SIZE
        debug call AssertError(node == 0, "deallocate()", Table(stack).string[-typeId], 0, "Null node")
        debug call AssertError(Table(stack)[offset + node] > 0, "deallocate()", Table(stack).string[-typeId], node, "Double-free")
        set Table(stack)[offset + node] = Table(stack)[offset]
        set Table(stack)[offset] = node
    endfunction

    module Alloc
        debug method operator allocated takes nothing returns boolean
            debug return IsAllocated(thistype.typeid, this)
        debug endmethod
        static method allocate takes nothing returns thistype
            return Allocate(thistype.typeid)
        endmethod
        method deallocate takes nothing returns nothing
            call Deallocate(thistype.typeid, this)
        endmethod
        debug private static method onInit takes nothing returns nothing
            debug set Table(stack).string[-thistype.typeid] = "thistype"
        debug endmethod
    endmodule

    module GlobalAlloc
        debug method operator allocated takes nothing returns boolean
            debug return IsAllocated(0, this)
        debug endmethod
        static method allocate takes nothing returns thistype
            debug call AssertError(Table(stack)[0] == (JASS_MAX_ARRAY_SIZE - 1), "allocate()", "thistype", JASS_MAX_ARRAY_SIZE - 1, "Overflow")
            return Allocate(0)
        endmethod
        method deallocate takes nothing returns nothing
            debug call AssertError(this == 0, "deallocate()", "thistype", 0, "Null node")
            debug call AssertError(Table(stack)[this] > 0, "deallocate()", "thistype", this, "Double-free")
            call Deallocate(0, this)
        endmethod
    endmodule


endlibrary