local gameHallController = class()

function gameHallController:ctor(  )
	self.gameHallView = require("MVC/GameHall/GameHallView"):new()
end

function gameHallController:startGame(  )
	print("startGame")
	initPosition = Vector3(9.0, -1.2 , -7.0)
	NetworkManager.Instance:Send(Common.Protocol.StartGameProtocol(userData.id, initPosition.x, initPosition.y, initPosition.z, 0, 0, 0))
end

function gameHallController:showView(  )
	global.UIManager:addView(self.gameHallView)
end

function gameHallController:closeView(  )
	global.UIManager:removeView(self.gameHallView)
end

function gameHallController:InitGameScene( param )
	local startGameProtocol = Common.Protocol.StartGameProtocol()
	startGameProtocol:Decode(param)
	customTransform = Common.CustomTransform(startGameProtocol.CTF.X,
            startGameProtocol.CTF.Y, startGameProtocol.CTF.Z, startGameProtocol.CTF.RX, startGameProtocol.CTF.RY, startGameProtocol.CTF.RZ)
	global.sceneManager:generateTank(startGameProtocol.Role_Id,customTransform)
	global.FSM:changeState("FSM/BattleState")
end

function gameHallController:registerProto(  )
	NetworkManager.Instance:RegisterProto(ProtocolId.StartGame, packFunction(self,self.InitGameScene))
end

function gameHallController:unregisterProto(  )
	NetworkManager.Instance:UnregisterProto(ProtocolId.StartGame, packFunction(self,self.InitGameScene))
end

return gameHallController