require("Class")

global = {
	
	FSM = require("FSM/FSM"):new(),

	UIManager = require("MVC/UIManager"):new(),

	loginController = require("MVC/Login/LoginController"):new(),

	registerController = require("MVC/Register/RegisterController"):new(),

	gameHallController = require("MVC/GameHall/GameHallController"):new(),

	battleController = require("MVC/Battle/BattleController"):new(),

	sceneManager = require("SceneManager/SceneManager"):new(),

	

}

userData = {}
tanks = {}

function packFunction( obj, method )
	if not obj[method] then
		obj[method] = function ( param )
			method(obj, param)
		end
	end
	return obj[method]
end