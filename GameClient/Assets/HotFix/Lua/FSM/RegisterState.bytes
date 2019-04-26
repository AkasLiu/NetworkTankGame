local  registerState = class(require "FSM/BaseState")

function registerState:enter( )
	global.registerController:showView()	
	global.registerController:registerProto()
end

function registerState:exit(  )
	global.registerController:closeView()
	global.registerController:unregisterProto()
end

return registerState