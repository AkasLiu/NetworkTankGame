local singleton = class()

function singleton:Instance(  )
	if self.instance == nil then
		self.instance=self:new()
	end
	return self.instance
end

return singleton