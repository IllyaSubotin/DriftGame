using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIStateMachine
{
    private UIState _currentState;
    private UIState _previousState;
    private readonly DiContainer _container;

    public UIStateMachine(DiContainer container)
    {
        _container = container;
    }

    public void ChangeState<T>() where T : UIState
    {
        var newState = _container.Resolve<T>();
        ChangeState(newState);
    }

    public void ChangeState(UIState newState)
    {
        _currentState?.Exit();

        _previousState = _currentState;
        _currentState = newState;

        _currentState?.Enter();
    }
}
