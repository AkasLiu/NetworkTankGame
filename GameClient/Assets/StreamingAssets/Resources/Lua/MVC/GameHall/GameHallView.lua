local gameHallView = class(require"MVC/Base/BaseView")

function gameHallView:start(  )
	self.gameHallViewPanelPrefab = Resources.Load("Prefabs/UI/GameHallView")
	self.gameHallViewPanel = GameObject.Instantiate(self.gameHallViewPanelPrefab,global.UIManager.UICanvas.transform)
	self.usernameText = GameObject.Find("UserNameText"):GetComponent("Text")
	self.startGameButton = GameObject.Find("StartGameButton"):GetComponent("Button")
    self.logoutButton = GameObject.Find("LogoutButton"):GetComponent("Button")

    self.usernameText.text = "角色名："..userData.username
    self.startGameButton.onClick:AddListener(packFunction(self,self.OnClickStartGameBtn))
    self.logoutButton.onClick:AddListener(packFunction(self, self.OnClickLogoutButton))
end

function gameHallView:OnClickStartGameBtn(  )
	global.gameHallController:startGame()
end

function gameHallView:OnClickLogoutButton(  )
	global.FSM:changeState("FSM/LoginState")
end

function gameHallView:destroy(  )
	GameObject.Destroy(self.gameHallViewPanel)
end

return gameHallView