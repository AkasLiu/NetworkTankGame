using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginView : ViewBase
{
    private GameObject loginViewPanelPrefab;
    private GameObject loginViewPanel;
    private InputField usernameInputField;
    private InputField passwordInputField;
    private Button loginButton = null;

    public LoginView()
    {
        //Start();
    }

    public override void Start()
    {
        base.Start();

        // 1. 实例化一个 prefab
        // 2. 搜索并持有所需控件
        // 3. 绑定控件的回调

        loginViewPanelPrefab = Resources.Load("Prefabs/UI/LoginView") as GameObject;

        loginViewPanel = Object.Instantiate(loginViewPanelPrefab, UIManager.Instance.UICanvas.transform);
        usernameInputField = loginViewPanel.transform.Find("UsernameLabel/UsernameInputField").GetComponent<InputField>();
        passwordInputField = loginViewPanel.transform.Find("PasswordLabel/PasswordInputField").GetComponent<InputField>();
        loginButton = loginViewPanel.transform.Find("LoginButton").GetComponent<Button>();

        loginButton.onClick.AddListener(OnClickLoginBtn);
    }

    void OnClickLoginBtn()
    {
        // 点击登录
        string username = usernameInputField.text;
        string pwd = passwordInputField.text;

        LoginController.Instance.Login(username, pwd);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        Object.DestroyImmediate(loginViewPanel);
    } 

}
