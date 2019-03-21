using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHallView : ViewBase {

    private GameObject gameHallViewPanelPrefab;
    private GameObject gameHallViewPanel;
    Button startGameButton;
    Button logoutButton;

    public GameHallView()
    {
        //Start();
    }

    public override void Start()
    {
        base.Start();

        // 1. 实例化一个 prefab
        // 2. 搜索并持有所需控件
        // 3. 绑定控件的回调
        gameHallViewPanelPrefab = Resources.Load("Prefabs/UI/GameHallView") as GameObject;

        gameHallViewPanel = Object.Instantiate(gameHallViewPanelPrefab, UIManager.Instance.UICanvas.transform);
        startGameButton = GameObject.Find("StartGameButton").GetComponent<Button>();
        logoutButton = GameObject.Find("LogoutButton").GetComponent<Button>();

        startGameButton.onClick.AddListener(OnClickStartGameBtn);
        logoutButton.onClick.AddListener(OnClickLogoutButton);
    }

    void OnClickStartGameBtn()
    {
        GameHallController.Instance.StartGame();
    }

    void OnClickLogoutButton()
    {
        StateMachine.Instance.ChangeState(StateType.Login);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        Object.Destroy(gameHallViewPanel);
    }



}
