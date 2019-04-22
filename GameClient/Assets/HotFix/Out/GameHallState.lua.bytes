local gameHallState = class(require "FSM/BaseState")

function gameHallState:enter( )
	global.gameHallController:showView()
	global.gameHallController:registerProto()
end

function gameHallState:exit(  )
	global.gameHallController:closeView()
	global.gameHallController:unregisterProto()
end

return gameHallState