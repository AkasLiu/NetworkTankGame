using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleView : ViewBase {

    private GameObject battleViewPanelPrefab = Resources.Load("Prefabs/UI/BattleView") as GameObject;
    //private GameObject joystickPanelPrefab=Resources.Load("Prefabs")
    private GameObject joystick;
    private GameObject battleViewPanel;    
    private Button exitGameButton;
    private Button fireButton;
   

    public BattleView()
    {
        //Start();
    }

    public override void Start()
    {
        base.Start();

        // 1. 实例化一个 prefab
        // 2. 搜索并持有所需控件
        // 3. 绑定控件的回调
        battleViewPanelPrefab = Resources.Load("Prefabs/UI/BattleView") as GameObject;

        battleViewPanel = Object.Instantiate(battleViewPanelPrefab, UIManager.Instance.UICanvas.transform);
        joystick = GameObject.Find("JoystickPanel/Joystick");
        joystick.AddComponent<EasyTouch>();
        exitGameButton = GameObject.Find("ExitGameButton").GetComponent<Button>();
        fireButton=GameObject.Find("FireButton").GetComponent<Button>();

        exitGameButton.onClick.AddListener(OnClickExitGameBtn);
        fireButton.onClick.AddListener(OnClickFireBtn);
    }

    void OnClickExitGameBtn()
    {
        BattleController.Instance.ExitGame();      
    }

    //
    void OnClickFireBtn()
    {
        BattleController.Instance.Fire();
    }

    //移动的事件
    public Vector3 move()
    {
        return Vector3.down;
    }


    public override void OnDestroy()
    {
        base.OnDestroy();

        Object.Destroy(battleViewPanel);
    }

}
