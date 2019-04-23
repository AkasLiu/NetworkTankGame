local loginView = class(require"MVC/Base/BaseView")

function loginView:start( )
	
	self.loginViewPanelPrefab = Resources.Load("Prefabs/UI/LoginView")
	self.loginViewPanel = GameObject.Instantiate(self.loginViewPanelPrefab,global.UIManager.UICanvas.transform)
	self.usernameInputField = GameObject.Find("UsernameLabel/UsernameInputField"):GetComponent("InputField")
	self.passwordInputField = GameObject.Find("PasswordLabel/PasswordInputField"):GetComponent("InputField")
	self.loginButton = GameObject.Find("LoginButton"):GetComponent("Button")

	self.loginButton.onClick:AddListener(packFunction(self, self.OnclickLoginBtn))

end

function loginView:OnclickLoginBtn( )
	self.username = self.usernameInputField.text
	self.pwd = self.passwordInputField.text

	global.loginController:login()
end

function loginView:destroy( )
	GameObject.Destroy(self.loginViewPanel)
end

return loginView