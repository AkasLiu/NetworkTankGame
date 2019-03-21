using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Protocol;
using Common;

public class GameHallController : Singleton<GameHallController>
{
    GameHallView view;
    //GameHallModel model;

    public GameHallController()
    {
        view = new GameHallView();
        //model = new GameHallModel();
    }

    //click startGamebutton ,send proto to server
    public void StartGame()
    {
        //1,set creation psotion
        //2,send
        Vector3 position = new Vector3(9.0f, -1.2f, -7.0f);
        NetworkManager.Instance.Send(new StartGameProtocol(PlayerInfoManager.Instance.GetUserData().ID, position.x, position.y, position.z, 0, 0, 0));
    }

    public void RegisterProto()
    {
        NetworkManager.Instance.RegisterProto((int)ProtocolId.StartGame, InitGameScene);
    }

    public void UnregisterProto()
    {
        NetworkManager.Instance.UnregisterProto((int)ProtocolId.StartGame, InitGameScene);
    }

    //定义一个函数 内容创建场景中的物体
    public void InitGameScene(object param)
    {        
        byte[] data = (byte[])param;
        Debug.Log("创建出场景");
        StartGameProtocol startGameProtocol = new StartGameProtocol();
        startGameProtocol.Decode(data);
        CustomTransform customTransform = new CustomTransform(startGameProtocol.CTF.X,
            startGameProtocol.CTF.Y, startGameProtocol.CTF.Z, startGameProtocol.CTF.RX, startGameProtocol.CTF.RY, startGameProtocol.CTF.RZ);
        StateMachine.Instance.ChangeState(StateType.Battle);
        SceneManager.Instance.GenerateTank(startGameProtocol.Role_Id, customTransform);
    }

    public void ShowView()
    {
        UIManager.Instance.AddView(view);
    }

    public void CloseView()
    {
        UIManager.Instance.RemoveView(view);
    }

}
