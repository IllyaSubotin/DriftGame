using UnityEngine;
using Zenject;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIGameplayManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _levelFinishPanel;
    [SerializeField] private GameObject _playerHubPanel;
    [SerializeField] private GameObject _newRecordPanel;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _rewardDoubleButton;
    [SerializeField] private TMP_Text _rewardScoreText;
    [SerializeField] private TMP_Text _rewardCashText;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _timerText;

    private float _displayScore = 0f;
    private float _smoothSpeed = 3f;

    private IGameController _gameController;
    private IScoreController _scoreManager;
    private IMoneyManager _moneyManager;
    private ITimerOnline _timerOnline;
    private SaveLoadManager _saveLoadManager;
    private IOnlineService _onlineService;
    private LevelPlayAdsManager _levelPlayAdsManager;
    private IOfflineTimer _offlineTimer;

    [Inject]
    private void Construct(IGameController gameController, IScoreController scoreManager, ITimerOnline timer, IOfflineTimer offlineTimer, 
                            IMoneyManager moneyManager, IOnlineService onlineService, LevelPlayAdsManager levelPlayAdsManager, SaveLoadManager saveLoadManager)
    {
        _gameController = gameController;
        _onlineService = onlineService;
        _saveLoadManager = saveLoadManager;
        _levelPlayAdsManager = levelPlayAdsManager;
        _scoreManager = scoreManager;
        _moneyManager = moneyManager;
        _offlineTimer = offlineTimer;
        _timerOnline = timer;

    }

    private void Start()
    {
        LevelStart();


        _scoreManager.OnScoreChanged += UpdateScore;
        
        if (_onlineService.isOnlineMode)
        {
            _timerOnline.OnTimeChanged += UpdateTimer;
            _timerOnline.OnTimerEnded += LevelFinish;
        }
        else
        {
            _offlineTimer.OnTimeChanged += UpdateTimer;
            _offlineTimer.OnTimerEnded += LevelFinish;
        }
    }
    
    private void LevelStart()
    {
        _playerHubPanel.SetActive(true);
        _levelFinishPanel.SetActive(false);
    }

    private void UpdateScore(float currentScore)
    {
        float realScore = _scoreManager.currentScore;

        _displayScore = Mathf.Lerp(_displayScore, realScore, Time.deltaTime * _smoothSpeed);

        _scoreText.text = Mathf.RoundToInt(_displayScore).ToString();
    }

    private void UpdateTimer(float timeLeft)
    {
        int m = Mathf.FloorToInt(timeLeft / 60f);
        int s = Mathf.FloorToInt(timeLeft % 60f);
        _timerText.text = $"{m:00}:{s:00}";
    }

    private void LevelFinish() 
    {
        _playerHubPanel.SetActive(false);
        _levelFinishPanel.SetActive(true);

        _rewardScoreText.text = "Очки: " + _scoreManager.currentScore;
        _rewardCashText.text = "Деньги: " + _scoreManager.currentScore / 10f;

        _moneyManager.Add(_scoreManager.currentScore / 10, MoneyType.Cash);
        _saveLoadManager.SaveGameData();

        _mainMenuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MainMenu");
        });

        _restartButton.onClick.AddListener(() =>
        {
            _gameController.LevelRestartOffline();
            LevelStart();
        });

        _rewardDoubleButton.onClick.AddListener(() =>
        {
            _levelPlayAdsManager.ShowRewarded();
            _moneyManager.Add(_scoreManager.currentScore / 10, MoneyType.Cash);
            _rewardCashText.text = "Деньги: " + _scoreManager.currentScore / 10f;
            _saveLoadManager.SaveGameData();
        });


    }

}