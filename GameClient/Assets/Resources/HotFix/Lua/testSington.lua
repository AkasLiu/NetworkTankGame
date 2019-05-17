local test = class()

function test:Instance(  )
	if self.instance == nil then
		self.instance=self:new()
	end
	return self.instance
end

test.x=1



return test