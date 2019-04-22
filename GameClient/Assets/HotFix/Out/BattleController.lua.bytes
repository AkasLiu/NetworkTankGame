local  battleController = class()

function battleController:ctor(  )
	self.battleView = require("MVC/Battle/BattleView"):new()
end

function battleController:generateTank( param )
	local startGameProtocol = Common.Protocol.StartGameProtocol()
	startGameProtocol:Decode(param)
	customTransform = Common.CustomTransform(startGameProtocol.CTF.X,
            startGameProtocol.CTF.Y, startGameProtocol.CTF.Z, startGameProtocol.CTF.RX, startGameProtocol.CTF.RY, startGameProtocol.CTF.RZ)
	global.sceneManager:generateTank(startGameProtocol.Role_Id,customTransform)
end

function battleController:registerProto( )
	NetworkManager.Instance:RegisterProto(ProtocolId.JoinGame, packFunction(self,self.generateTank))
end

function battleController:unregisterProto(  )
	NetworkManager.Instance:UnregisterProto(ProtocolId.JoinGame, packFunction(self,self.generateTank))
end

--[[function battleController:exitGame(  )	
	global.FSM:changeState("FSM/GameHallState")
end--]]

function battleController:fire(  )
	--local fireProtocol = Common.Protocol.FireProtocol(userData.id)
	NetworkManager.Instance:Send(Common.Protocol.FireProtocol(userData.id))
end

function battleController:showView(  )
	global.UIManager:addView(self.battleView)
end

function battleController:closeView(  )
	global.UIManager:removeView(self.battleView)
end

return battleController