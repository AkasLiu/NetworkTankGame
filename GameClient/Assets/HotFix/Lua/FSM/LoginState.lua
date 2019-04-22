local  loginState = class(require "FSM/BaseState")

function loginState:enter( )
	global.loginController:showView()	
	global.loginController:registerProto()
end

function loginState:exit(  )
	global.loginController:closeView()
	global.loginController:unregisterProto()
end

return loginState

