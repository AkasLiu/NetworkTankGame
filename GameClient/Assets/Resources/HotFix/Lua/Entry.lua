import "UnityEngine"

require("Utils")

local entry = {}

local  FSM = global.FSM
local UIManager = global.UIManager

function main(  )
	UIManager:start()
    FSM:start()  

  	return entry
end

function entry:update(  )
	global.UIManager:update()
end
