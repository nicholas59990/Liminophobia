using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private Dictionary<StateTypes, IState> states = new Dictionary<StateTypes, IState>();
    private IState currentState;
    public StateTypes GetCurrentStateType()
    {
        return currentState.Type;
    }
    public void AddState(IState state)
    {
        if (!states.ContainsKey(state.Type))
        {
            states.Add(state.Type, state);
        }
    }
    public void TransitionToState(StateTypes type)
    {
        currentState?.Exit();
        currentState = states[type];
        currentState.Enter();
    }
    public void Update()
    {
        currentState?.Execute();
    }
}
