using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : Singleton<StateMachine> {

    private List<StateBase> m_StateList = new List<StateBase>();
    private Dictionary<StateType, StateBase> m_StateDict = new Dictionary<StateType, StateBase>();

    //1，初始化state字典 2，将login状态加入到list，并enter 状态
    public void Start()
    {
        m_StateDict.Add(StateType.Login, new LoginState());
        m_StateDict.Add(StateType.GameHall, new GameHallState());
        m_StateDict.Add(StateType.Battle, new BattleState());

        m_StateList.Add(FindState(StateType.Login));
        FindState(StateType.Login).Enter();        
    }

    //改变状态
    public void ChangeState(StateType stateType)
    {
        StateBase state = FindState(stateType);
        if (state != null)
        {
            m_StateList[m_StateList.Count - 1].Exit();
            m_StateList.Remove(m_StateList[m_StateList.Count - 1]);
            state.Enter();
            m_StateList.Add(state);
        }
        else
        {
            Debug.Log("state is null");
            return;
        }

    }

    public StateType GetCurrentState()
    {
        return m_StateList[m_StateList.Count - 1].GetStateType();
    }

    //根据stateType 查询字典中是否有该state
    private StateBase FindState(StateType stateType)
    {
        StateBase state = null;
        if (!m_StateDict.TryGetValue(stateType, out state))
        {
            Debug.LogError("不存在次状态");
            return null;
        }
        return state;
    }

}
