local FSM = class()
local loginState = require("FSM/LoginState")
local gameHallState = require("FSM/GameHallState")
local battleState = require("FSM/BattleState")
local registerState = require("FSM/RegisterState")

function FSM:ctor(  )
	self.loginState = loginState:new()
	self.registerState = registerState:new()
	self.gameHallState = gameHallState:new()
	self.battleState = battleState:new()
	self.currentState = self.loginState
end

function FSM:start( )	
	self.currentState.enter()
end

function FSM:changeState( stateType )
	self.currentState:exit()

	--local path = tostring("FSM/"+stateType)

	local path =tostring(stateType)
	local  nextState = require(path):new()
	nextState:enter()

	self.currentState = nextState
end

return FSM