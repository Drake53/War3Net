library TilesInRange  /* v1.1 -- hiveworkshop.com/threads/tilesinrange.277289/


*/ requires /*

        */ WorldBounds    /* github.com/nestharus/JASS/blob/master/jass/Systems/WorldBounds/script.j
        */ TileDefinition /* hiveworkshop.com/forums/jass-resources-412/snippet-tiledefinition-259347/

   
    Information
    ¯¯¯¯¯¯¯¯¯¯¯
   
            Allows to enumerate through tiles in range.
            There is currently no code for resetting OP limit,
            so I don't know if for example too much tiles in a huge map can be added.
       
       
       
    Applied method
    ¯¯¯¯¯¯¯¯¯¯¯¯¯¯
            Rules for tile inclusion:
 
                1. A tile gets included when it's TileMinimum is included.
                2. A tile gets NOT included when only it's TileMaximum is included.
                    (because it's maximum is also the minimum of the very next tile, and so comes Rule 1)
                3. If any cooridnate from the tile is in range, the tile will be included. If the center is in range does not matter.
                    (exception is Rule 2, of course)
                4. Tiles outside the WorldBounds will be excluded.
   
            Example:
   
                Tile1: minX=-64   maxX= 64   minY =-64   maxY=64
                Tile2: minX= 64   maxX=192   minY =-64   maxY=64
       
            Now we add Tile(64/60) to the list, but which Tile will be included?
                Tile1 will not be included, because 64 is it's X maximum.
                Tile2 will be included, because 64 is it's X minimum.
           
           
            These rules are based on how TileDefinition works.
           
            In case you want a stricter filtering, for example only include tiles which's TileCenter is in range
            you have the possibility to do so with custom filters. See API.
 
*/
//! novjass
 //=================== --------- API --------- ====================


    function GetTilesInRange takes real x, real y, real range, boolexpr filter returns TileData
   
        // returns the first element to start your loop. (see later)
        // if the returned TileData equals "0" , the list is empty.
   
        // If a list already exist, the system will automaticaly clear and destroy it on this function call.
        // This means you can only have one list at a time, but also don't have
        // to care about desotroying it yourself/leaks.
   
 
    struct TileData
 
        integer id
        real range
 
//      You are able to enumerate though the List like this:
   
            local TileData this = GetTilesInRange(x, y, 100, null) // null filter
            loop
                exitwhen(this == 0)
                    // this.id      // TileId of this
                    // this.range   // Range of the tile to x/y
                set this = this.next
            endloop
       
        static thistype first    // you can start loops with it
        static integer  Size     // Size of the list.
        static integer  TileId   // The current Tile Id which you refer to inside the filter.
        static integer  Range    // The current Tile Range which you refer to inside the filter.
 
//      Example:
   
            function MyFilter takes nothing returns boolean
                call BJDebugMsg("Current Tile id: " + I2S(TileData.TileId) + " | Range: " + R2S(TileData.Range))
            endfunction


 //=================== ---------------------- ====================
//! endnovjass

globals
    private trigger handler = CreateTrigger()
    private triggercondition tc = null
endglobals

// only need a very basic structure, which is fast
private module List
 
    readonly thistype next
    readonly static thistype first = 0
    readonly static integer Size = 0
 
    method add takes nothing returns nothing
        if(thistype.Size > 8190) then
            debug call BJDebugMsg("TilesInRange - Module List: Max amount of elements has been reached \"8191\".")
            return
        endif
        set this.next = thistype.first
        set thistype.first = this
        set thistype.Size = thistype.Size + 1
    endmethod
 
    static method destroyList takes nothing returns nothing
        local thistype this = thistype.first
        loop
            exitwhen (this == 0)
            call this.destroy()
            set this = this.next
        endloop
        set thistype.first = 0
        set thistype.Size = 0
        if tc != null then
            call TriggerRemoveCondition(handler, tc)
            set tc = null
        endif
    endmethod
 
endmodule

struct TileData
    implement List
    real range
    integer id
 
    static integer TileId = 0
    static real Range = 0
 
    static method create takes nothing returns thistype
        local thistype this = thistype.allocate()
        call this.add()
        return this
    endmethod
endstruct

private function IsXInWorld takes real x returns boolean
    return x < WorldBounds.maxX and x > WorldBounds.minX
endfunction

private function IsYInWorld takes real y returns boolean
    return y < WorldBounds.maxY and y > WorldBounds.minY
endfunction

private function IsXYInWorld takes real x, real y returns boolean
    return x < WorldBounds.maxX and x > WorldBounds.minX and y < WorldBounds.maxY and y > WorldBounds.minY
endfunction

private function AddTile takes real x, real y, real r returns TileData
    local TileData this
    set TileData.TileId = GetTileId(x, y)
    set TileData.Range = r
    if IsXYInWorld(x, y) and TriggerEvaluate(handler)then
        set this = TileData.create()
        set this.id = TileData.TileId
        set this.range = TileData.Range
        return this
    endif
    return 0
endfunction

function GetTilesInRange takes real x, real y, real range, boolexpr filter  returns TileData
 
    local real tempX
    local real tempY
    local real maxX
    local real maxY
    local real yRange
    local real rangeSquare
    local real currentRange
    local real currentRangeTwo
    local TileData this
 
    if range < 0 then
        debug call BJDebugMsg("LIBRARY TilesInRange: Invalid input \"range\". Must be positive.")
        return 0
    endif
    if  not IsXYInWorld(x, y) then
        debug call BJDebugMsg("LIBRARY TilesInRange: Invalid input \"x\" or \"y\". Must be in world")
        return 0
    endif
 
    // So user never has to care about destroying lists
    if (TileData.first != 0) then
        call TileData.destroyList()
    endif
 
    if filter != null then
        set tc = TriggerAddCondition(handler, filter)
    endif
 
//     We will operate though tiles in range in 3 steps.
//     ¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
 
//  Step 1
 
//      We directly add tile at start location.
//      We go up and downwards from it, and add all tiles in range.
   
   
    call AddTile(x, y, 0)
 
    // Add all tiles above start tile
    set maxY = GetTileMax(y + range)
    set tempY = y + 128
    set currentRange = GetTileMax(y) - y
    loop
        exitwhen tempY >= maxY or not IsYInWorld(tempY)
        call AddTile(x, tempY, currentRange)
        set currentRange = currentRange + 128
        set tempY = tempY + 128
    endloop
   
    // Add all tiles under start tile
    set maxY = GetTileMin(y - range)
    set tempY =  y - 128
    set currentRange = y - GetTileMin(y)
    loop
        exitwhen tempY < maxY  or not IsYInWorld(tempY)
        call AddTile(x, tempY, currentRange)
        set currentRange = currentRange + 128
        set tempY = tempY - 128
    endloop
 
    set rangeSquare = range*range
 
//  Step 2
 
//      We go right and add all tiles in range.
//      We go up and downwards from all of them, and also add the tiles in range.
 
    set tempX = GetTileMax(x)
    set maxX = GetTileMax(x + range)
    set currentRange = GetTileMax(x) - x
    loop
        exitwhen tempX >= maxX  or not IsXInWorld(tempX)
        call AddTile(tempX, y, currentRange)
   
        set yRange = SquareRoot( rangeSquare - (tempX-x)*(tempX-x) )
   
        // Add all tiles above current tile
        set maxY = GetTileMax(y + yRange)
        set tempY = y + 128
        set currentRangeTwo = GetTileMax(y) - y
        loop
            exitwhen tempY >= maxY  or not IsXYInWorld(tempX, tempY)
            call AddTile(tempX, tempY, SquareRoot(currentRange*currentRange + currentRangeTwo*currentRangeTwo))
            set currentRangeTwo = currentRangeTwo + 128
            set tempY = tempY + 128
        endloop
   
        // Add all tiles under current tile
        set maxY = GetTileMin(y - yRange)
        set tempY =  y - 128
        set currentRangeTwo = y - GetTileMin(y)
        loop
            exitwhen tempY < maxY  or not IsXYInWorld(tempX, tempY)
            call AddTile(tempX, tempY, SquareRoot(currentRange*currentRange + currentRangeTwo*currentRangeTwo))
            set currentRangeTwo = currentRangeTwo + 128
            set tempY = tempY - 128
        endloop
   
        set currentRange = currentRange + 128
        set tempX = tempX + 128
    endloop
 
// Step 3
 
//     We go left and add all tiles in range.
//     We go up and downwards from all of them, and also add the tiles in range.
   
    set maxX = GetTileMin(x - range)
    set tempX = GetTileMin(x) - 128
    set currentRange = x - GetTileMin(x)
    loop
        exitwhen tempX < maxX  or not IsXInWorld(tempX)
        call AddTile(tempX, y, currentRange)
   
        set yRange = SquareRoot( rangeSquare - (tempX-x+128)*(tempX-x+128) )
   
        // Add all tiles above current tile
        set maxY = GetTileMax(y + yRange)
        set tempY = y + 128
        set currentRangeTwo = GetTileMax(y) - y
        loop
            exitwhen tempY >= maxY  or not IsXYInWorld(tempX, tempY)
            call AddTile(tempX, tempY, SquareRoot(currentRange*currentRange + currentRangeTwo*currentRangeTwo))
            set currentRangeTwo = currentRangeTwo + 128
            set tempY = tempY + 128
        endloop
   
        // Add all tiles under current tile
        set maxY = GetTileMin(y - yRange)
        set tempY =  y - 128
        set currentRangeTwo = y - GetTileMin(y)
        loop
            exitwhen tempY < maxY  or not IsXYInWorld(tempX, tempY)
            call AddTile(tempX, tempY, SquareRoot(currentRange*currentRange + currentRangeTwo*currentRangeTwo))
            set currentRangeTwo = currentRangeTwo + 128
            set tempY = tempY -128
        endloop
   
        set currentRange = currentRange + 128
        set tempX = tempX - 128
   
    endloop
    return TileData.first
endfunction

endlibrary