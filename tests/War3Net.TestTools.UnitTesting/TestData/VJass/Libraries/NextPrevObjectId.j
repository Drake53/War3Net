library NextPrevObjectId /* v1.0  https://www.hiveworkshop.com/threads/nextprev-objectid.313328/

    Provides API to receive an ObjectId's next and prev ObjectId.
 
    Example for using next ObjectId:
    'A009' -> 'A00A'
    'A00z' -> 'A010'
 
    ObjectIds in the object editor work with symbols: 0-9 , A-Z , a-z ... they are ASCII symbols.
    Other ASCII symbols are not being used. The problem would exist if we would use a common +1 loop
    instead of NextPrevObjectId function, then we would also get all the other ASCIIs, which we might not want.
 
    Example with common +1 loop:
    'A009' -> 'A00:'
    'A00z' -> 'A00{'
 
    ^This is usually an unwanted result, which we don't need in our ObjectId loop.
    So this library is supposed to help looping through valid ObjectIds, considering only the wanted ASCII symbols.
 
    API
 
    function GetNextObjectId takes integer objectId returns integer
    function GetPrevObjectId takes integer objectId returns integer

*/
    globals
        private integer array pow256
        private integer array gapValue
    endglobals

    private function GetNextObjectId_private takes integer objectId, integer bytenumber returns integer
        local integer currentByteValue
        if ( bytenumber > -1 and bytenumber < 4 ) then
            set currentByteValue = ModuloInteger(objectId, pow256[bytenumber + 1])  / pow256[bytenumber]
        else
            return objectId
        endif
        if ( currentByteValue != '9' and currentByteValue != 'Z' and currentByteValue != 'z' ) then
            return objectId + pow256[bytenumber]
        else
            if ( currentByteValue != 'z' ) then
                return objectId + gapValue[currentByteValue] * pow256[bytenumber]
            else
                return GetNextObjectId_private(objectId - gapValue[currentByteValue] * pow256[bytenumber], (bytenumber + 1))
            endif
        endif
    endfunction
 
    private function GetPrevObjectId_private takes integer objectId, integer bytenumber returns integer
        local integer currentByteValue
        if ( bytenumber > -1 and bytenumber < 4 ) then
            set currentByteValue = ModuloInteger(objectId, pow256[bytenumber + 1])  / pow256[bytenumber]
        else
            return objectId
        endif
        if ( currentByteValue != '0' and currentByteValue != 'A' and currentByteValue != 'a' ) then
            return objectId - pow256[bytenumber]
        else
            if ( currentByteValue != '0' ) then
                return objectId - gapValue[currentByteValue] * pow256[bytenumber]
            else
                return GetPrevObjectId_private(objectId + gapValue[currentByteValue] * pow256[bytenumber], (bytenumber + 1))
            endif
        endif
    endfunction
 
    function GetPrevObjectId takes integer objectId returns integer
        return GetPrevObjectId_private(objectId, 0)
    endfunction
 
    function GetNextObjectId takes integer objectId returns integer
        return GetNextObjectId_private(objectId, 0)
    endfunction
 
    private module M
        private static method onInit takes nothing returns nothing
            local integer i = 0
            loop
                exitwhen i > 4
                set pow256[i] = R2I(Pow(256, i))
                set i = i + 1
            endloop
            set gapValue['9'] = 'A' - '9'
            set gapValue['Z'] = 'a' - 'Z'
            set gapValue['z'] = 'z' - '0'
            set gapValue['A'] = gapValue['9']
            set gapValue['a'] = gapValue['Z']
            set gapValue['0'] = gapValue['z']
        endmethod
    endmodule
    private struct S extends array
        implement M
    endstruct
endlibrary