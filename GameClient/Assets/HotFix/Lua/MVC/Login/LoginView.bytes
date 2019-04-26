local loginView = class(require"MVC/Base/BaseView")

function loginView:start( )
	
	self.loginViewPanelPrefab = Resources.Load("Prefabs/UI/LoginView")
	self.loginViewPanel = GameObject.Instantiate(self.loginViewPanelPrefab,global.UIManager.UICanvas.transform)
	self.usernameInputField = self.loginViewPanel.transform:Find("UsernameInputField"):GetComponent("InputField")
	self.passwordInputField = self.loginViewPanel.transform:Find("PasswordInputField"):GetComponent("InputField")
	self.tip = self.loginViewPanel.transform:Find("Tip"):GetComponent("Text")
	self.loginButton = self.loginViewPanel.transform:Find("LoginButton"):GetComponent("Button")
	self.registerButton = self.loginViewPanel.transform:Find("RegisterButton"):GetComponent("Button")

	self.loginButton.onClick:AddListener(packFunction(self, self.OnclickLoginBtn))
	self.registerButton.onClick:AddListener(packFunction(self, self.OnclickRegisterBtn))
end

function loginView:OnclickLoginBtn( )
	self.username = self.usernameInputField.text
	self.pwd = self.passwordInputField.text

	global.loginController:login()
end

function loginView:OnclickRegisterBtn(  )
	global.FSM:changeState("FSM/RegisterState")
end

function loginView:destroy( )
	GameObject.Destroy(self.loginViewPanel)
end

function loginView:showTip( text )
	self.tip.text = text
end

return loginView