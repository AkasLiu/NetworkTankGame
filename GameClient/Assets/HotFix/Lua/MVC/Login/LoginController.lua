local loginController = class()

function loginController:ctor(  )
	self.loginView = require("MVC/Login/LoginView"):new()
end

function loginController:registerProto(  )
	NetworkManager.Instance:RegisterProto(ProtocolId.ReturnUserDataProtocol,packFunction(self,self.verify))
end

function loginController:unregisterProto(  )
	NetworkManager.Instance:UnregisterProto(ProtocolId.ReturnUserDataProtocol,packFunction(self,self.verify))
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
	local returnUserData = Common.Protocol.ReturnUserDataProtocol()
	returnUserData:Decode(param)

	if returnUserData.Result then
		userData.id =  returnUserData.Id
		userData.username =	returnUserData.Username
		global.FSM:changeState("FSM/GameHallState")
	end

end

return  loginController
