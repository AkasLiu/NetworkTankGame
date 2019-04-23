local  FSM = class()
local loginState = require("FSM/LoginState")
local gameHallState = require("FSM/GameHallState")
local battleState = require("FSM/BattleState")

function FSM:ctor(  )
	self.loginState = loginState:new()
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

function FSM:FindState( stateType )
	--[[for k,v in ipairs(self) do
		if(type(v) == stateType)
			break
		end
	end--]]
end

return FSM