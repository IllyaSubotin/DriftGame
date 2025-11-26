using Zenject;
using UnityEngine;

public class MainMenuBootstrap : MonoBehaviour
{
    private UIStateMachine _stateMachine;
    private SaveLoadManager _saveLoadManager;

    [Inject]
    private void Construct(UIStateMachine stateMachine, SaveLoadManager saveLoadManager)
    {
        _stateMachine = stateMachine;
        _saveLoadManager = saveLoadManager;
    }
    
    private void Start()
    {
        _saveLoadManager.LoadGameData();
        _stateMachine.ChangeState<MainMenuState>();
    }
}
