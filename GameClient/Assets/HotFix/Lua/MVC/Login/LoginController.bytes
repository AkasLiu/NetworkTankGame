local loginController = class()

function loginController:ctor(  )
	self.loginView = require("MVC/Login/LoginView"):new()
end

function loginController:registerProto(  )
	NetworkManager.Instance:RegisterProto(ProtocolID.ReturnResult,packFunction(self,self.verify))
	NetworkManager.Instance:RegisterProto(ProtocolID.ReturnUserDataProtocol,packFunction(self,self.saveUserData))
end

function loginController:unregisterProto(  )
	NetworkManager.Instance:UnregisterProto(ProtocolID.ReturnResult,packFunction(self,self.verify))
	NetworkManager.Instance:UnregisterProto(ProtocolID.ReturnUserDataProtocol,packFunction(self,self.saveUserData))
end

function loginController:login(  )
	NetworkManager.Instance:Send(Common.Protocol.LoginProtocol(self.loginView.username, self.loginView.pwd))
end

function loginController:showView(  )
	global.UIManager:addView(self.loginView)
end

function loginController:closeView(  )
	global.UIManager:removeView(self.loginView)
end

function loginController:verify( param )
	local resultProtocol = Common.Protocol.ResultProtocol()
	resultProtocol:Decode(param)
	if resultProtocol.Result then 
		self.loginView:showTip("登陆成功")
	else 
		self.loginView:showTip("登陆失败，账号或密码错误")
	end
end

function loginController:saveUserData( param )
	local returnUserData = Common.Protocol.ReturnUserDataProtocol()
	returnUserData:Decode(param)
	userData.id =  returnUserData.Id
	userData.username =	returnUserData.Username
	global.FSM:changeState("FSM/GameHallState")
end

return  loginController
