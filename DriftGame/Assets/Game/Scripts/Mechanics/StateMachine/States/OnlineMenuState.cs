using Zenject;

public class OnlineMenuState : UIState
{
    private OnlineMenuScreen _screen;
    private UIStateMachine _stateMachine;
    private IMultiplayerManager _multiplayerManager;
    [Inject]
    public void Construct(OnlineMenuScreen onlineMenuScreen, UIStateMachine stateMachine, IMultiplayerManager multiplayerManager)
    {
        _screen = onlineMenuScreen;
        _stateMachine = stateMachine;
        _multiplayerManager = multiplayerManager;
    }

    public override void Enter()
    {
        _screen.Show();
        _multiplayerManager.Connect();

        _screen.createRoomButton.onClick.AddListener(() =>
        {
            _multiplayerManager.CreateRoom();
        });

        _screen.joinRoomButton.onClick.AddListener(() =>
        {
            _multiplayerManager.JoinRandomRoom();
        });

        _screen.backButton.onClick.AddListener(() =>
        {
            _stateMachine.ChangeState<MainMenuState>();
        });
    }

    public override void Exit()
    {
        _screen.Hide();
    }
}
