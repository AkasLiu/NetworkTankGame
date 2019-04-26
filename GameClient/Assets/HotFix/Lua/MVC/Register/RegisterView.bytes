local registerView = class(require"MVC/Base/BaseView")

function registerView:start(  )
	self.registerViewPanelPrefab = Resources.Load("Prefabs/UI/RegisterView")
	self.registerViewPanel = GameObject.Instantiate(self.registerViewPanelPrefab,global.UIManager.UICanvas.transform)
	self.usernameInputField = self.registerViewPanel.transform:Find("UsernameInputField"):GetComponent("InputField")
	self.passwordInputField =self.registerViewPanel.transform:Find("PasswordInputField"):GetComponent("InputField")
	self.rePasswordInputField = self.registerViewPanel.transform:Find("RePasswordInputField"):GetComponent("InputField")
	self.tip = self.registerViewPanel.transform:Find("Tip"):GetComponent("Text")
	self.registerButton = self.registerViewPanel.transform:Find("RegisterButton"):GetComponent("Button")
	self.exitButton = self.registerViewPanel.transform:Find("ExitButton"):GetComponent("Button")

	self.registerButton.onClick:AddListener(packFunction(self, self.OnclickRegisterBtn))
	self.exitButton.onClick:AddListener(packFunction(self, self.OnclickExitBtn))
end

function registerView:OnclickRegisterBtn(  )
	self.username = self.usernameInputField.text
	self.pwd = self.passwordInputField.text
	self.repwd = self.rePasswordInputField.text
	
	global.registerController:register()
end

function registerView:OnclickExitBtn(  )
	global.FSM:changeState("FSM/LoginState")
end

function registerView:showTip( text )
	self.tip.text = text
end

function registerView:destroy( )
	GameObject.Destroy(self.registerViewPanel)
end

return registerView