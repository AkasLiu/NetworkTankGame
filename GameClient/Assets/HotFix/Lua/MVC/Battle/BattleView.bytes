local  battleView = class(require"MVC/Base/BaseView")

function battleView:start( )
	self.battleViewPanelPrefab = Resources.Load("Prefabs/UI/BattleView")
	self.battleViewPanel = GameObject.Instantiate(self.battleViewPanelPrefab,global.UIManager.UICanvas.transform)
	self.jostick = GameObject.Find("JoystickPanel/Joystick")
	self.jostick:AddComponent("EasyTouch")
	self.easytouch = self.jostick:GetComponent("EasyTouch")
	self.exitGameButton = GameObject.Find("ExitGameButton"):GetComponent("Button")
	self.fireButton = GameObject.Find("FireButton"):GetComponent("Button")

	self.exitGameButton.onClick:AddListener(packFunction(self,self.OnClickExitGameBtn))
	self.fireButton.onClick:AddListener(packFunction(self,self.OnClickFireBtn))
end

function battleView:OnClickExitGameBtn(  )
	global.FSM:changeState("FSM/GameHallState")
end

function battleView:OnClickFireBtn(  )
	global.battleController:fire()
end

function battleView:destroy(  )
	GameObject.Destroy(self.battleViewPanel)
end

function battleView:update(  )
	local horizontal = self.jostick.transform.localPosition.x
	local vertical = self.jostick.transform.localPosition.y

	if tanks[userData.id] ~= nil then 
		if math.abs(horizontal) >0 or math.abs(vertical)>0 then
			global.sceneManager:onRecevieMove(userData.id,horizontal,vertical)
		end
	end

	if tanks[userData.id] ~= nil then
		local tank = tanks[userData.id].gameObject
		syncPositionProtocol = Common.Protocol.SyncPositionProtocol(userData.id, 
			tank.transform.position.x, tank.transform.position.y, tank.transform.position.z,
			tank.transform.localEulerAngles.x, tank.transform.localEulerAngles.y, tank.transform.localEulerAngles.z)
		NetworkManager.Instance:Send(syncPositionProtocol)
	end
end

return battleView