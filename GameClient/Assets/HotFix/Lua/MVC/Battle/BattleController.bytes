local  battleController = class()

function battleController:ctor(  )
	self.battleView = require("MVC/Battle/BattleView"):new()
end

function battleController:generateTank( param )
	local joinGameProtocol = Common.Protocol.JoinGameProtocol()
	joinGameProtocol:Decode(param)
	local myTransform = Common.MyTransform(joinGameProtocol.mytf.X,
            joinGameProtocol.mytf.Y, joinGameProtocol.mytf.Z, joinGameProtocol.mytf.RX, joinGameProtocol.mytf.RY, joinGameProtocol.mytf.RZ)
	global.sceneManager:generateTank(joinGameProtocol.Role_Id,myTransform)
end

function battleController:registerProto( )
	NetworkManager.Instance:RegisterProto(ProtocolID.JoinGame, packFunction(self,self.generateTank))
end

function battleController:unregisterProto(  )
	NetworkManager.Instance:UnregisterProto(ProtocolID.JoinGame, packFunction(self,self.generateTank))
end

function battleController:fire(  )
	NetworkManager.Instance:Send(Common.Protocol.FireProtocol(userData.id))
end

function battleController:showView(  )
	global.UIManager:addView(self.battleView)
end

function battleController:closeView(  )
	global.UIManager:removeView(self.battleView)
end

return battleController