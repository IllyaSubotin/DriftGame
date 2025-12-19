using Zenject;
using UnityEngine;
using Photon.Pun;

public class GameController : MonoBehaviour, IGameController
{
    public Transform[] spawnPoints;
    public PhotonView photonView;
    public float levelDuration = 120f;


    private GameObject _car;

    private IScoreController _scoreController;
    private ICameraController _cameraController;
    private IOnlineService _onlineService;
    private IMultiplayerManager _multiplayerManager;
    private ICarFactory _factory;
    private ITimerOnline _timerOnline;
    private IOfflineTimer _timerOffline;
    

    [Inject]
    private void Construct(ICarFactory carFactory, ITimerOnline timer, IScoreController scoreController,
                            ICameraController cameraController, IOfflineTimer timerOffline, IOnlineService onlineService,
                                IMultiplayerManager multiplayerManager)
    {
        _scoreController = scoreController;
        _timerOffline = timerOffline;
        _cameraController = cameraController;
        _multiplayerManager = multiplayerManager;
        _onlineService = onlineService;
        _factory = carFactory;
        _timerOnline = timer;
        
    }

    private void Start()
    {
        if (_onlineService.isOnlineMode)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                LevelStartOnline();
            }
            else
            {
                _multiplayerManager.OnOnlineRoomJoined += LevelStartOnline; 
            }

        }
        else LevelStartOffline();
    }

    private void OnDisable()
    {
        _multiplayerManager.OnOnlineRoomJoined -= LevelStartOnline;
    }

    private void LevelStartOffline()
    {
        var car = _factory.SpawnPlayerCar(spawnPoints[0]);

        ScoreControllerInit(car);

        _cameraController.SetTarget(car.transform);
        _timerOffline.StartTimer(levelDuration);
    }

    private void LevelStartOnline()
    {
        var playerId = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        var car = _factory.SpawnPlayerCarOnline(spawnPoints[playerId]);

        ScoreControllerInit(car);

        _timerOnline.StartTimer(levelDuration);
        _cameraController.SetTarget(car.transform);        
    }

    public void LevelRestartOffline()
    {
        ResetCarPosition();

        _scoreController.RestartScore();
        _timerOffline.StartTimer(levelDuration);
    }
    public void LevelRestartOnline()
    {
        
    }

    private void ScoreControllerInit(GameObject car)
    {
        var controller = car.GetComponent<ICarController>();
            _scoreController.Init(controller);
    }


    private void ResetCarPosition()
    {
        _car.transform.position = spawnPoints[0].position;
        _car.transform.rotation = spawnPoints[0].rotation;
    }   

    [PunRPC] private void StartRace()
    {
        _timerOnline.StartTimer(120f);
    }
    
}
