local oldSubString = SubString
SubString = function(source, start, _end)
    if start > StringLength(source) then
        return nil
    end
    return oldSubString(source, start, _end)
end
