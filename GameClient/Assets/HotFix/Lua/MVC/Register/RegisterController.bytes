local registerController = class()

function registerController:ctor(  )
	self.registerView = require("MVC/Register/RegisterView"):new()
end

function registerController:registerProto(  )
	NetworkManager.Instance:RegisterProto(ProtocolID.ReturnResult,packFunction(self,self.verify))
end

function registerController:unregisterProto(  )
	NetworkManager.Instance:UnregisterProto(ProtocolID.ReturnResult,packFunction(self,self.verify))
end

function registerController:register(  )
	print("r")
	if self.registerView.pwd == self.registerView.repwd then
		NetworkManager.Instance:Send(Common.Protocol.RegisterProtocol(self.registerView.username, self.registerView.pwd))
	else
		self.registerView:showTip("两次密码输入不一致")
	end
end

function registerController:showView(  )
	global.UIManager:addView(self.registerView)
end

function registerController:closeView(  )
	global.UIManager:removeView(self.registerView)
end

function registerController:verify( param )

	local resultProtocol = Common.Protocol.ResultProtocol()
	resultProtocol:Decode(param)
	if resultProtocol.Result then 
		self.registerView:showTip("注册成功")
	else 
		self.registerView:showTip("注册失败，账号已存在")
	end
end

return  registerController