library VirtualGrid requires /*
    ------------------
    */ Table        /*
    ------------------
        -> Bribe
        
        link: https://www.hiveworkshop.com/threads/snippet-new-table.188084/
    
     -------------------
    |   VirtualGrid     |
    |     v.1.2.0       |
    |                   |
    |     - MyPad       |
     -------------------
  
     ---------------------------------------
    |                                       |
    |   A library that makes                |
    |   virtual partitioning logic          |
    |   easier to handle                    |
    |                                       |
    |   Practical use:                      |
    |       - Missile systems               |
     ---------------------------------------
   
     ---------------------------------------------------
    |                                                   |
    |   Credits:                                        |
    |       - Pinzu for suggesting                      |
    |         improvements on the capabilities          |
    |         of the library (such as instantiation     |
    |         of individual VirtualGrids                |
    |                                                   |
    |       - Overfrost for suggesting the inclusion    |
    |         of circular grids.                        |
    |                                                   |
     ---------------------------------------------------
    
     ----------------------------------------------------------------------
    |                                                                         
    |   API:
    |
    |   -------------------------------------------------------------------
    |
    |   Definition:
    |       Parent Instance -   A VirtualGrid object created via createForRect or
    |                           createForCircle
    |
    |       Child Instance  -   A VirtualGrid "id" that is bound to the Parent
    |                           Instance. They can only be accessed by
    |                           parent instances, and are not
    |                           forcibly allocated by the system.
    |
    |   -------------------------------------------------------------------
    |
    |   struct VirtualGrid extends array
    |
    |       -------------
    |       Constructors:
    |       -------------
    |       
    |       static method createFromRect(rect whichRect, integer cols, integer rows) -> VirtualGrid
    |           - Generates a unique VirtualGrid Parent Instance based on the given rect
    |             handle.
    |
    |       static method createFromCircle(real x, real y, real radius, integer cols, integer rows) -> VirtualGrid
    |           - Generates a unique VirtualGrid Parent Instance based on the given circle.
    |
    |       -----------
    |       Destructor:
    |       -----------
    |
    |       method destroy()
    |           - Destroys a VirtualGrid Parent Instance. (Child Instances cannot be destroyed this way)
    |             If attempting to destroy Child Instances, this will print an error.
    |
    |       ----------------
    |       Utility Methods:
    |       ----------------
    |
    |       ------------------------------------------------------------
    |       Note: The following methods do not accept Child Instances:
    |       ------------------------------------------------------------
    |
    |       method getGridFromPoint(real x, y) -> VirtualGrid
    |           - Retrieves a VirtualGrid Child Instance based on the coordinates.
    |           - In debug mode, this will apply bounds checking
    |             so that if out-of-bounds, it will return 0 and
    |             display an error.
    |
    |       method isPointInGrid(real x, real y, VirtualGrid subGrid) -> boolean
    |           - Checks if a point lies in the specified VirtualGrid subGrid
    |           - The note above applies here. (A Child Version of the method
    |             exists below)
    |
    |       ------------------------------------------------------------
    |       Note: The following methods accept both Parent and Child Instances.
    |       ------------------------------------------------------------
    |
    |       method operator minX
    |       method operator minY
    |       method operator maxX
    |       method operator maxY
    |           - Returns the necessary values dynamically computed at runtime.
    |           - Sub-Grids can invoke this function.
    |
    |       ------------------------------------------------------------
    |       Note: The following methods do not accept parent instances in SAFE_MODE
    |       ------------------------------------------------------------
    |
    |       method getLeftGrid() -> VirtualGrid
    |           - Returns the left grid adjacent to the requesting grid.
    |           - For leftmost grids, this will return themselves.
    |
    |       method getRightGrid() -> VirtualGrid
    |           - Returns the right grid adjacent to the requesting grid.
    |           - For rightmost grids, this will return themselves.
    |
    |       method getTopGrid() -> VirtualGrid
    |           - Returns the top grid adjacent to the requesting grid.
    |           - For topmost grids, this will return themselves.
    |
    |       method getBottomGrid() -> VirtualGrid
    |           - Returns the bottom grid adjacent to the requesting grid.
    |           - For bottom-most grids, this will return themselves.
    |
    |       ------------------------------------------------------------
    |
    |       method isPointWithinGrid(real x, real y) -> boolean
    |           - The child version of :isPointInGrid()
    |           - Does not accept Parent Instances even when not in SAFE_MODE.
    |
     ---------------------------------------------------------------------
    |
    |   globals
    |       SAFE_MODE   - A flag determining whether the system will
    |                     enforce Parent/Child instance safety when
    |                     running.
    |
    |                     This applies to methods where
    |                     Parent Instances are not accepted, and
    |                     in the checking of the provided rect handle
    |                     in constructor createFromRect, and of the
    |                     radius in constructor createFromCircle.
    |
    |       GRID_TYPE_RECT      - A unique enum variable. (Must not be 0)
    |       GRID_TYPE_CIRCLE    - A unique enum variable. (Must not be 0
    |
     ---------------------------------------------------------------------
*/
    
globals
    private constant boolean SAFE_MODE              = false
    
    private constant integer GRID_TYPE_RECT         = 1
    private constant integer GRID_TYPE_CIRCLE       = 2
endglobals
static if DEBUG_MODE then
private function Error takes string consoleName, string msg, string errorType returns nothing
    call DisplayTimedTextToPlayer(GetLocalPlayer(), 0, 0, 50000, "|cffff0000>> " + consoleName + "|r -> " + msg + " |cffff0000" + errorType + "|r")
endfunction
endif
private module Init
    private static method onInit takes nothing returns nothing
        call thistype.init()
    endmethod
endmodule
struct VirtualGrid extends array
    private static thistype array allocNodes
    private static Table allocTable
    
    private integer gridType
    private integer size
    private boolean isHead
    
    //  For rects
    readonly rect quad
    readonly real quadCellX
    readonly real quadCellY
    readonly integer quadRows
    readonly integer quadCols
    
    //  For circles
    readonly real centerX
    readonly real centerY
    readonly real radius
    readonly real radStep
    readonly integer centerRows     //  The angular partitions
    readonly integer centerCols     //  The concentric partitions
    
    method destroy takes nothing returns nothing
        if not this.isHead then
            debug call Error("thistype:destroy", "Cannot destroy Child Instance or a deallocated Parent Instance", "InvalidObjectException")
            return
        endif
        set this.isHead = false
        
        if this.gridType == GRID_TYPE_RECT then
            set this.quad       = null
            set this.quadCellX  = 0.
            set this.quadCellY  = 0.
        else    // if this.gridType == GRID_TYPE_CIRCLE then
            set this.centerX    = 0.
            set this.centerY    = 0.
            set this.radius     = 0.
            set this.centerCols = 0
            set this.centerRows = 0
        endif
        
        set allocTable[this.size].integer[integer(this)] = allocTable[this.size].integer[0]
        set allocTable[this.size].integer[0]             = integer(this)
        
        set this.size   = 0
    endmethod
    //  Core constructor
    private static method create takes integer size returns thistype
        local thistype object = 0
        
        if size <= 0 then
            return object
        endif
        
        if not allocTable.integer.has(size) then
            set allocTable[size]    = Table.create()
        endif
        
        set object = thistype(allocTable[size].integer[0])
        if allocTable[size].integer[object] == 0 then
            //  No deallocated instance to be reallocated
            set object                      = thistype(allocTable.integer[0] + 1)
            set allocTable.integer[0]       = integer(object) + size
            
            set allocNodes[0]               = thistype(integer(allocNodes[0]) + 1)
            set allocNodes[allocNodes[0]]   = object
        else
            set allocTable[size].integer[0]     = allocTable[size].integer[integer(object)]
            call allocTable[size].integer.remove(integer(object))        
        endif
        set object.isHead   = true
        set object.size     = size
        return object
    endmethod
    
    //  Members generated to not waste locals.
    private static thistype array extras        
    private static real array vars
    
    private method getHeader takes nothing returns nothing
        set extras[0] = 0
        set extras[1] = thistype(allocNodes[0])
        set extras[2] = thistype(((extras[0]) + (extras[1]))/2)
        set extras[3] = 0
        loop
            if ((this) < (allocNodes[extras[2]])) then
                set extras[1] = allocNodes[extras[2]]
            else
                set extras[0] = allocNodes[extras[2]]
            endif
            set extras[3] = extras[2]
            set extras[2] = thistype(((extras[0]) + (extras[1]))/2)
            
            exitwhen extras[3] == extras[2]
        endloop
    endmethod
    
    private method minimax takes nothing returns nothing
        set vars[0] = I2R(ModuloInteger((this) - (extras[2]) - 1, extras[2].centerRows) + 1)*extras[2].radStep
        set vars[1] = I2R(R2I((extras[2].size + (extras[2]) - (this))/I2R(extras[2].centerRows)))
    endmethod
    
    method operator minX takes nothing returns real
        if this.gridType == GRID_TYPE_RECT then
            return GetRectMinX(this.quad)
        elseif this.gridType == GRID_TYPE_CIRCLE then
            return extras[2].centerX - extras[2].radius
        endif
        call this.getHeader()
        if extras[2].gridType == GRID_TYPE_RECT then
            return GetRectMinX(extras[2].quad) + (ModuloInteger(((this) - (extras[2])) - 1, extras[2].quadCols)+1)*extras[2].quadCellX
        endif
        call this.minimax()
        if (vars[0] > bj_PI/2) and (3*bj_PI/2 > vars[0]) then
            if vars[0] <= bj_PI then
                return extras[2].centerX + (extras[2].radius*((vars[1]+1)/I2R(extras[2].centerCols)))*Cos(vars[0])
            endif
            return extras[2].centerX + (extras[2].radius*((vars[1]+1)/I2R(extras[2].centerCols)))*Cos(vars[0] - extras[2].radStep)
        endif
        if (vars[0] <= bj_PI/2) then
            return extras[2].centerX + (extras[2].radius*((vars[1])/I2R(extras[2].centerCols)))*Cos(vars[0])
        endif
        return extras[2].centerX + (extras[2].radius*((vars[1]+1)/I2R(extras[2].centerCols)))*Cos(vars[0] - extras[2].radStep)
    endmethod
    
    method operator maxX takes nothing returns real
        if this.gridType == GRID_TYPE_RECT then
            return GetRectMaxX(this.quad)
        elseif this.gridType == GRID_TYPE_CIRCLE then
            return extras[2].centerX + extras[2].radius            
        endif
        call this.getHeader()
        if extras[2].gridType == GRID_TYPE_RECT then
            return GetRectMinX(extras[2].quad) + (ModuloInteger(((this) - (extras[2])) - 1, extras[2].quadCols)+2)*extras[2].quadCellX
        endif
        call this.minimax()
        if (vars[0] > bj_PI/2) and (3*bj_PI/2 > vars[0]) then
            if vars[0] <= bj_PI then
                return extras[2].centerX + (extras[2].radius*((vars[1])/I2R(extras[2].centerCols)))*Cos(vars[0] - extras[2].radStep)
            endif
            return extras[2].centerX + (extras[2].radius*((vars[1])/I2R(extras[2].centerCols)))*Cos(vars[0])
        endif
        if (vars[0] <= bj_PI/2) then
            return extras[2].centerX + (extras[2].radius*((vars[1]+1)/I2R(extras[2].centerCols)))*Cos(vars[0] - extras[2].radStep)
        endif
        return extras[2].centerX + (extras[2].radius*((vars[1]+1)/I2R(extras[2].centerCols)))*Cos(vars[0])
    endmethod
    
    method operator minY takes nothing returns real
        if this.gridType == GRID_TYPE_RECT then
            return GetRectMinY(this.quad)
        elseif this.gridType == GRID_TYPE_CIRCLE then
            return extras[2].centerY - extras[2].radius
        endif
        call this.getHeader()
        if extras[2].gridType == GRID_TYPE_RECT then
            return GetRectMaxY(extras[2].quad) - (((this) - (extras[2]))/extras[2].quadRows)*extras[2].quadCellY
        endif
        call this.minimax()
        if (vars[0] <= bj_PI) then
            if (vars[0] <= bj_PI/2) then
                return extras[2].centerY + (extras[2].radius*((vars[1])/I2R(extras[2].centerCols)))*Sin(vars[0] - extras[2].radStep)
            endif
            return extras[2].centerY + (extras[2].radius*((vars[1])/I2R(extras[2].centerCols)))*Sin(vars[0])
        endif
        if (vars[0] <= 3*bj_PI/2) then
            return extras[2].centerY + (extras[2].radius*((vars[1]+1)/I2R(extras[2].centerCols)))*Sin(vars[0])
        endif
        return extras[2].centerY + (extras[2].radius*((vars[1]+1)/I2R(extras[2].centerCols)))*Sin(vars[0] - extras[2].radStep)
    endmethod
    method operator maxY takes nothing returns real
        if this.gridType == GRID_TYPE_RECT then
            return GetRectMaxY(this.quad)
        elseif this.gridType == GRID_TYPE_CIRCLE then
            return extras[2].centerY + extras[2].radius
        endif
        call this.getHeader()
        if extras[2].gridType == GRID_TYPE_RECT then
            return GetRectMaxY(extras[2].quad) - ((((this) - (extras[2]))/extras[2].quadRows) - 1)*extras[2].quadCellY
        endif
        call this.minimax()
        if (vars[0] <= bj_PI) then
            if (vars[0] <= bj_PI/2) then
                return extras[2].centerY + (extras[2].radius*((vars[1]+1)/I2R(extras[2].centerCols)))*Sin(vars[0])
            endif
            return extras[2].centerY + (extras[2].radius*((vars[1]+1)/I2R(extras[2].centerCols)))*Sin(vars[0] - extras[2].radStep)
        endif
        if (vars[0] <= 3*bj_PI/2) then
            return extras[2].centerY + (extras[2].radius*((vars[1])/I2R(extras[2].centerCols)))*Sin(vars[0] - extras[2].radStep)
        endif
        return extras[2].centerY + (extras[2].radius*((vars[1])/I2R(extras[2].centerCols)))*Sin(vars[0])
    endmethod
    
    method getRightGrid takes nothing returns thistype
        static if SAFE_MODE then
            if this.isHead then
                debug call Error("thistype:getRightGrid", "Parent Instance was provided (this) (" + I2S(this) + ")", "(InvalidObjectException)")
                return this
            endif
        endif
        call this.getHeader()
        if extras[2].gridType == GRID_TYPE_RECT then
            if ModuloInteger((this) - (extras[2]) + 1, extras[2].quadCols) == 0 then
                //  Rightmost instance
                debug call Error("thistype:getRightGrid", "Instance provided is already situated at the rightmost section", "(OutOfBoundsException)")
                return this
            endif
            return thistype((this) + 1)
        endif
        if ModuloInteger((this) - (extras[2]) - 1, extras[2].centerRows) == 0 then
            return thistype((this) + extras[2].centerRows - 1)
        endif
        return thistype((this) - 1)
    endmethod
    
    method getLeftGrid takes nothing returns thistype
        static if SAFE_MODE then
            if this.isHead then
                debug call Error("thistype:getLeftGrid", "Parent Instance was provided (this) (" + I2S(this) + ")", "(InvalidObjectException)")
                return this
            endif
        endif
        call this.getHeader()
        if extras[2].gridType == GRID_TYPE_RECT then
            if ModuloInteger((this) - (extras[2]) - 1, extras[2].quadCols) == 0 then
                //  Leftmost instance
                debug call Error("thistype:getLeftGrid", "Instance provided is already situated at the leftmost section", "(OutOfBoundsException)")
                return this
            endif
            return thistype((this) - 1)
        endif
        if ModuloInteger((this) - (extras[2]) + 1, extras[2].centerRows) == 0 then
            return thistype((this) - extras[2].centerRows + 1)
        endif
        return thistype((this) + 1)    
    endmethod
    
    method getTopGrid takes nothing returns thistype
        static if SAFE_MODE then
            if this.isHead then
                debug call Error("thistype:getTopGrid", "Parent Instance was provided (this) (" + I2S(this) + ")", "(InvalidObjectException)")
                return this
            endif
        endif
        call this.getHeader()
        if extras[2].gridType == GRID_TYPE_RECT then
            if ((this) - (extras[2]))/extras[2].quadRows <= 0 then
                //  Topmost instance
                debug call Error("thistype:getTopGrid", "Instance provided is already situated at the topmost section", "(OutOfBoundsException)")
                return this
            endif
            return thistype((this) - extras[2].quadRows)
        endif
        if R2I(I2R((extras[2]) + extras[2].size - (this))/I2R(extras[2].centerRows)) + 1 >= extras[2].centerCols then
            return this
        endif
        return thistype((this) - extras[2].centerRows)
    endmethod
    
    method getBottomGrid takes nothing returns thistype
        static if SAFE_MODE then
            if this.isHead then
                debug call Error("thistype:getBottomGrid", "Parent Instance was provided (this) (" + I2S(this) + ")", "(InvalidObjectException)")
                return this
            endif
        endif
        call this.getHeader()
        if extras[2].gridType == GRID_TYPE_RECT then
            if ((this) - (extras[2]))/extras[2].quadRows <= 0 then
                //  Topmost instance
                debug call Error("thistype:getBottomGrid", "Instance provided is already situated at the bottommost section", "(OutOfBoundsException)")
                return this
            endif
            return thistype((this) - extras[2].quadRows)
        endif
        if R2I(I2R((this) - (extras[2]))/I2R(extras[2].centerRows)) + 1 >= extras[2].centerCols then
            return this
        endif
        return thistype((this) + extras[2].centerRows)
    endmethod
    
    method getGridFromPoint takes real x, real y returns thistype
        if not this.isHead then
            debug call Error("thistype:getGridFromPoint", "Child Instance was provided (this) (" + I2S(this) + ")", "(InvalidObjectException)")
            return 0
        endif
        if this.gridType == GRID_TYPE_RECT then
            debug if x < this.minX or x > this.maxX then
                debug call Error("thistype:getGridFromPoint", "Point out of bounds of rect (this) (" + I2S(this) + ")", "(OutOfBoundsError)")
                debug return 0
            debug endif
            return thistype((this) + R2I((x - this.minX)/this.quadCellX) + 1 + R2I((this.maxY - y)/this.quadCellY)*this.quadRows)
        endif
        
        set vars[0] = Atan2(y - this.centerY, x - this.centerX)
        
        set vars[1] = (y - this.centerY)*(y - this.centerY) + (x - this.centerX)*(x - this.centerX)
        set vars[2] = 1./I2R(this.centerCols)
        set vars[3] = vars[2]
        set vars[4] = this.radius*this.radius
        
        if vars[0] < 0 then
            set vars[0] = vars[0] + 2*bj_PI
        endif
        loop
            exitwhen vars[3]*vars[3]*vars[4] > vars[1]
            set vars[3] = vars[3] + vars[2]
        endloop
        if vars[3] > 1 then
            debug call Error("thistype:getGridFromPoint", "Point out of bounds for circle (this) (" + I2S(this) + ")", "(OutOfBoundsError)")
            return 0
        endif
        set vars[3] = vars[3]*this.centerCols
        return thistype((this) + R2I(vars[0]/this.radStep) + 1 + (this.centerCols - R2I(vars[3]))*this.centerRows)
    endmethod
    
    method isPointInGrid takes real x, real y, thistype subGrid returns boolean
        if not this.isHead then
            debug call Error("thistype:isPointInGrid", "This method cannot be accessed by Child Instances.", "(InvalidObjectException)")
            return false
        endif
        return this.getGridFromPoint(x, y) == subGrid
    endmethod
    
    method isPointWithinGrid takes real x, real y returns boolean
        if this.isHead then
            debug call Error("thistype:isPointWithinGrid", "This method cannot be accessed by Parent Instances.", "(InvalidObjectException)")
            return false
        endif
        call this.getHeader()
        return extras[2].getGridFromPoint(x, y) == this
    endmethod
    
    static method createFromRect takes rect whichRect, integer cols, integer rows returns thistype
        local thistype object = 0
        static if SAFE_MODE then
            if whichRect == null then
                debug call Error("thistype::createFromRect", "parameter whichRect (in line 507) has no value", "(NullHandleException)")
                return object
            endif
        endif
        
        set object = thistype.create(cols*rows)
        //  In debug mode, this will point out that the grid you're making has invalid parameters
        debug if object == 0 then
            debug call Error("thistype::createFromRect", "The resulting number of grids requested is less than 0.", "(GridCountError)")
        debug endif
        
        if object != 0 then
            //  Ensure that invalid reads are not generated at runtime.
            set object.gridType     = GRID_TYPE_RECT
            set object.quad         = whichRect
            set object.quadRows     = rows
            set object.quadCols     = cols
            set object.quadCellX    = (GetRectMaxX(whichRect) - GetRectMinX(whichRect))/cols
            set object.quadCellY    = (GetRectMaxY(whichRect) - GetRectMinY(whichRect))/rows
        endif        
        return object
    endmethod
    static method createFromCircle takes real x, real y, real radius, integer cols, integer rows returns thistype
        local thistype object = 0
        static if SAFE_MODE then
            if radius <= 0. then
                debug call Error("thistype::createFromCircle", "parameter radius (in line 535) is invalid", "(ExtraneousInputException)")
                return object
            endif
        endif
        
        set object = thistype.create(cols*rows)
        //  In debug mode, this will point out that the grid you're making has invalid parameters
        debug if object == 0 then
            debug call Error("thistype::createFromCircle", "The resulting number of grids requested is less than 0.", "(GridCountError)")
        debug endif
        
        if object != 0 then
            //  Ensure that invalid reads are not generated at runtime.
            set object.gridType     = GRID_TYPE_CIRCLE
            set object.centerX      = x
            set object.centerY      = y
            set object.radius       = radius
            set object.centerRows   = rows
            set object.centerCols   = cols
            set object.radStep      = (2*bj_PI)/rows
        endif  
        return object
    endmethod
    
    private static method init takes nothing returns nothing
        set thistype.allocTable        = Table.create()
    endmethod
    
    implement Init
endstruct
endlibrary