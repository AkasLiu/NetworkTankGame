local  battleState = class(require "FSM/BaseState")

function battleState:enter(  )
	print("enter battle")
	global.battleController:showView()
	global.battleController:registerProto()
	global.sceneManager:start()
end

function battleState:exit(  )
	print("exit battle")
	global.battleController:closeView()
	global.battleController:unregisterProto()
	global.sceneManager:exit()
end

return battleState