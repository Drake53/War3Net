type location extends handle

native GetLocationX takes location loc returns real
native GetLocationY takes location loc returns real

function CompareLocationsBJ takes location A, location B returns boolean
    return GetLocationX(A) == GetLocationX(B) and GetLocationY(A) == GetLocationY(B)
	// Gets parsed as: GetLocationX(A) == (GetLocationX(B) and (GetLocationY(A) == GetLocationY(B)))
endfunction
