library MouseUtils
/*
    -------------------
        MouseUtils
         - MyPad
         
         1.3.1
    -------------------
    
    ----------------------------------------------------------------------------
        A simple snippet that allows one to
        conveniently use the mouse natives
        as they were meant to be...
        
     -------------------
    |    API            |
     -------------------
    
        struct UserMouse extends array
            static method operator [] (player p) -> thistype
                - Returns the player's id + 1
                
            static method getCurEventType() -> integer
                - Returns the custom event that got executed.
                
            method operator player -> player
                - Returns Player(this - 1)
                
            readonly real mouseX
            readonly real mouseY
                - Returns the current mouse coordinates.
                
            readonly method operator isMouseClicked -> boolean
                - Determines whether any mouse key has been clicked,
                  and will return true on the first mouse key.
                  
            method isMouseButtonClicked(mousebuttontype mouseButton)
                - Returns true if the mouse button hasn't been
                  released yet.
            method setMousePos(real x, y) (introduced in 1.0.2.2)
                - Sets the mouse position for a given player.
                  
            static method registerCode(code c, integer ev) -> triggercondition
                - Lets code run upon the execution of a certain event.
                - Returns a triggercondition that can be removed later.
                
            static method unregisterCallback(triggercondition trgHndl, integer ev)
                - Removes a generated triggercondition from the trigger.
                
        functions:
            GetPlayerMouseX(player p) -> real
            GetPlayerMouseY(player p) -> real
                - Returns the coordinates of the mouse of the player.
                
            OnMouseEvent(code func, integer eventId) -> triggercondition
                - See UserMouse.registerCode
                
            GetMouseEventType() -> integer
                - See UserMouse.getCurEventType
                
            UnregisterMouseCallback(triggercondition t, integer eventId)
                - See UserMouse.unregisterCallback
            SetUserMousePos(player p, real x, real y)
                - See UserMouse.setMousePos
    
  Unique Global Constants:
   IMPL_LOCK (Introduced in v.1.0.2.2)
    - Enables or disables the lock option
     -------------------
    |    Credits        |
     -------------------
     
        -   Pyrogasm for pointing out a comparison logic flaw
            in operator isMouseClicked.
            
        -   Illidan(Evil)X for the useful enum handles that
            grant more functionality to this snippet.
        
        -   TriggerHappy for the suggestion to include 
            associated events and callbacks to this snippet.
            
        -   Quilnez for pointing out a bug related to the
            method isMouseButtonClicked not working as intended
            in certain situations.
        -   Lt_Hawkeye for pointing out a bug related to the
            registration of callback functions not working
            as intended. (The triggers that would hold these
            functions did not exist yet at those moments)
            
    ----------------------------------------------------------------------------
*/
//  Arbitrary constants
globals
    constant integer EVENT_MOUSE_UP     = 0x400
    constant integer EVENT_MOUSE_DOWN   = 0x800
    constant integer EVENT_MOUSE_MOVE   = 0xC00
    //  Introduced in v1.0.2.3
    //  Commented out in v1.0.2.4
    // private constant real STARTUP_DELAY = 0.00
    // private constant boolean NO_DELAY   = false
    //  Introduced in v1.0.2.2
    private constant boolean IMPL_LOCK  = true
endglobals
private module Init
    private static method invokeTimerInit takes nothing returns nothing
        call PauseTimer(GetExpiredTimer())
        call DestroyTimer(GetExpiredTimer())
        call thistype.timerInit()
    endmethod
    private static method onInit takes nothing returns nothing
        set evTrigger[EVENT_MOUSE_UP]   = CreateTrigger()
        set evTrigger[EVENT_MOUSE_DOWN] = CreateTrigger()
        set evTrigger[EVENT_MOUSE_MOVE] = CreateTrigger()
        call TimerStart(CreateTimer(), 0.00, false, function thistype.invokeTimerInit)
    endmethod
endmodule
struct UserMouse extends array
    static if IMPL_LOCK then
        //  Determines the minimum interval that a mouse move event detector
        //  will be deactivated. (Globally-based)
        //  You can configure it to any amount you like.
        private static constant real INTERVAL               = 0.031250000
        
        //  Determines how many times a mouse move event detector can fire
        //  before being deactivated. (locally-based)
        //  You can configure this to any integer value. (Preferably positive)
        private static constant integer MOUSE_COUNT_MAX     = 16
        
        // Determines the amount to be deducted from mouseEventCount
        // per INTERVAL. Runs independently of resetTimer
        private static constant integer MOUSE_COUNT_LOSS    = 8
        private static constant boolean IS_INSTANT          = (INTERVAL <= 0.)
    endif
    private static integer currentEventType             = 0
    private static integer updateCount                  = 0
    private static trigger stateDetector                = null
    static if IMPL_LOCK and not IS_INSTANT then
        private static timer resetTimer                 = null
        private integer  mouseEventCount
        private timer mouseEventReductor
    endif
    private static trigger array evTrigger
    private static integer array mouseButtonStack
 
    private thistype next
    private thistype prev
    
    private thistype resetNext
    private thistype resetPrev
    private trigger posDetector
    private integer mouseClickCount
    
    readonly real mouseX
    readonly real mouseY
    
    //  Converts the enum type mousebuttontype into an integer
    private static method toIndex takes mousebuttontype mouseButton returns integer
        return GetHandleId(mouseButton)
    endmethod
    
    static method getCurEventType takes nothing returns integer
        return currentEventType
    endmethod
    
    static method operator [] takes player p returns thistype
        if thistype(GetPlayerId(p) + 1).posDetector != null then
            return GetPlayerId(p) + 1
        endif
        return 0
    endmethod
        
    method operator player takes nothing returns player
        return Player(this - 1)
    endmethod
    method operator isMouseClicked takes nothing returns boolean
        return .mouseClickCount > 0
    endmethod
    method isMouseButtonClicked takes mousebuttontype mouseButton returns boolean
        return UserMouse.mouseButtonStack[(this - 1)*3 + UserMouse.toIndex(mouseButton)] > 0
    endmethod
    method setMousePos takes integer x, integer y returns nothing
        if GetLocalPlayer() == this.player then
            call BlzSetMousePos(x, y)
        endif
    endmethod
    static if IMPL_LOCK then
    private static method getMouseEventReductor takes timer t returns thistype
        local thistype this = thistype(0).next
        loop
        exitwhen this.mouseEventReductor == t or this == 0
            set this = this.next
        endloop
        return this
    endmethod
    private static method onMouseUpdateListener takes nothing returns nothing
        local thistype this = thistype(0).resetNext
        set updateCount     = 0
        
        loop
            exitwhen this == 0
            set updateCount = updateCount + 1
                        
            set this.mouseEventCount        = 0
            call EnableTrigger(this.posDetector)
            
            set this.resetNext.resetPrev    = this.resetPrev
            set this.resetPrev.resetNext    = this.resetNext
            
            set this    = this.resetNext
        endloop
        if updateCount > 0 then
            static if not IS_INSTANT then
                call TimerStart(resetTimer, INTERVAL, false, function thistype.onMouseUpdateListener)
            else
                call onMouseUpdateListener() 
            endif
        else
            static if not IS_INSTANT then
                call TimerStart(resetTimer, 0.00, false, null)
                call PauseTimer(resetTimer)
            endif
        endif
    endmethod
    private static method onMouseReductListener takes nothing returns nothing
        local thistype this  = getMouseEventReductor(GetExpiredTimer())
        if this.mouseEventCount <= 0 then
            call PauseTimer(this.mouseEventReductor)
        else
            set this.mouseEventCount = IMaxBJ(this.mouseEventCount - MOUSE_COUNT_LOSS, 0)
            call TimerStart(this.mouseEventReductor, INTERVAL, false, function thistype.onMouseReductListener)
        endif
    endmethod
    endif
    private static method onMouseUpOrDown takes nothing returns nothing
        local thistype this = thistype[GetTriggerPlayer()]
        local integer index = (this - 1)*3 + UserMouse.toIndex(BlzGetTriggerPlayerMouseButton())
        local boolean releaseFlag   = false
        
        if GetTriggerEventId() == EVENT_PLAYER_MOUSE_DOWN then
            set this.mouseClickCount    = IMinBJ(this.mouseClickCount + 1, 3)
            set releaseFlag          = UserMouse.mouseButtonStack[index] <= 0
            set UserMouse.mouseButtonStack[index]  = IMinBJ(UserMouse.mouseButtonStack[index] + 1, 1)
           
            if releaseFlag then
                set currentEventType = EVENT_MOUSE_DOWN
                call TriggerEvaluate(evTrigger[EVENT_MOUSE_DOWN])
            endif
        else
            set this.mouseClickCount = IMaxBJ(this.mouseClickCount - 1, 0)
            set releaseFlag          = UserMouse.mouseButtonStack[index] > 0
            set UserMouse.mouseButtonStack[index]  = IMaxBJ(UserMouse.mouseButtonStack[index] - 1, 0)
            
            if releaseFlag then
                set currentEventType = EVENT_MOUSE_UP
                call TriggerEvaluate(evTrigger[EVENT_MOUSE_UP])
            endif
        endif
    endmethod
    
    private static method onMouseMove takes nothing returns nothing
        local thistype this   = thistype[GetTriggerPlayer()]
        local boolean started  = false
        set this.mouseX      = BlzGetTriggerPlayerMouseX()
        set this.mouseY      = BlzGetTriggerPlayerMouseY()
        static if IMPL_LOCK then
            set this.mouseEventCount  = this.mouseEventCount + 1
            if this.mouseEventCount <= 1 then
                call TimerStart(this.mouseEventReductor, INTERVAL, false, function thistype.onMouseReductListener)
            endif
        endif
        set currentEventType   = EVENT_MOUSE_MOVE
        call TriggerEvaluate(evTrigger[EVENT_MOUSE_MOVE])  
        static if IMPL_LOCK then
            if this.mouseEventCount >= thistype.MOUSE_COUNT_MAX then
                call DisableTrigger(this.posDetector)                  
                if thistype(0).resetNext == 0 then
                    static if not IS_INSTANT then
                        call TimerStart(resetTimer, INTERVAL, false, function thistype.onMouseUpdateListener)
                    // Mouse event reductor should be paused
                    else
                        set started  = true
                    endif
                    call PauseTimer(this.mouseEventReductor)
                endif
                set this.resetNext              = 0
                set this.resetPrev              = this.resetNext.resetPrev
                set this.resetPrev.resetNext    = this
                set this.resetNext.resetPrev    = this  
                if started then
                    call onMouseUpdateListener()
                endif
            endif
        endif
    endmethod
        
    private static method initCallback takes nothing returns nothing
        local thistype this = 1
        local player p      = this.player
  
        static if IMPL_LOCK and not IS_INSTANT then
            set resetTimer  = CreateTimer()
        endif
        set stateDetector   = CreateTrigger()
        call TriggerAddCondition( stateDetector, Condition(function thistype.onMouseUpOrDown))
        loop
            exitwhen integer(this) > bj_MAX_PLAYER_SLOTS
            if GetPlayerController(p) == MAP_CONTROL_USER and GetPlayerSlotState(p) == PLAYER_SLOT_STATE_PLAYING then
                set this.next             = 0
                set this.prev             = thistype(0).prev
                set thistype(0).prev.next = this
                set thistype(0).prev      = this
                
                set this.posDetector         = CreateTrigger()
                static if IMPL_LOCK and not IS_INSTANT then
                    set this.mouseEventReductor  = CreateTimer()
                endif
                call TriggerRegisterPlayerEvent( this.posDetector, p, EVENT_PLAYER_MOUSE_MOVE )
                call TriggerAddCondition( this.posDetector, Condition(function thistype.onMouseMove))                
                
                call TriggerRegisterPlayerEvent( stateDetector, p, EVENT_PLAYER_MOUSE_UP )
                call TriggerRegisterPlayerEvent( stateDetector, p, EVENT_PLAYER_MOUSE_DOWN )
            endif
            set this = this + 1
            set p    = this.player
        endloop
    endmethod
    
    private static method timerInit takes nothing returns nothing
        call thistype.initCallback()
    endmethod
    static method registerCode takes code handlerFunc, integer eventId returns triggercondition
        return TriggerAddCondition(evTrigger[eventId], Condition(handlerFunc))
    endmethod
    
    static method unregisterCallback takes triggercondition whichHandler, integer eventId returns nothing
        call TriggerRemoveCondition(evTrigger[eventId], whichHandler)
    endmethod
    
    implement Init
endstruct
function GetPlayerMouseX takes player p returns real
    return UserMouse[p].mouseX
endfunction
function GetPlayerMouseY takes player p returns real
    return UserMouse[p].mouseY
endfunction
function OnMouseEvent takes code func, integer eventId returns triggercondition
    return UserMouse.registerCode(func, eventId)
endfunction
function GetMouseEventType takes nothing returns integer
    return UserMouse.getCurEventType()
endfunction
function UnregisterMouseCallback takes triggercondition whichHandler, integer eventId returns nothing
    call UserMouse.unregisterCallback(whichHandler, eventId)
endfunction
function SetUserMousePos takes player p, integer x, integer y returns nothing
    call UserMouse[p].setMousePos(x, y)
endfunction
endlibrary