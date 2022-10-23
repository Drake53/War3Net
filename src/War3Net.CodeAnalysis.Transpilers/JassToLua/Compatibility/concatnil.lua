local mt = getmetatable("")
mt.__concat = function(left, right) return (left or "") .. (right or "") end
