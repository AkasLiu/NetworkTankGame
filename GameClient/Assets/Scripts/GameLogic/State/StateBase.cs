using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase {

    protected StateType currentState;

    public StateBase()
    {
        currentState = StateType.None;
    }

    public abstract void Enter();

    public abstract void Exit();

    public abstract void Excute();

    public StateType GetStateType()
    {
        return currentState;
    }
}
