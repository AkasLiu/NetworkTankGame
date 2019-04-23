singleTon = {}

function singleTon:new()
　　local store = nil
　　return 
	function(self)
	　　　　if store then return store end
	　　　　local o ={}
	　　　　setmetatable(o,self)
	　　　　self.__index = self	　　　　
	　　　　store = o
　　　　    return  o
　　　　end
end

return singleTon