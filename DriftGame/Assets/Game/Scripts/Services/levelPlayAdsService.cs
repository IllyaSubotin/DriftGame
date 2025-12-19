using UnityEngine;
using Unity.Services.LevelPlay;   // LevelPlay SDK (Init + Ad об'єкти)

public class LevelPlayAdsManager : MonoBehaviour
{

    [Header("App Keys")]
    [SerializeField] private string appKeyAndroid;

    [Header("Ad Unit IDs (з LevelPlay)")]
    [SerializeField] private string rewardedAdUnitId;

    // Об'єкти реклами
    private LevelPlayRewardedAd rewardedAd;

    private void Start()
    {
        InitLevelPlay();
    }

    private void InitLevelPlay()
    {
        LevelPlay.OnInitSuccess += OnLevelPlayInitSuccess;

        string appKey = appKeyAndroid;

        // userId можна передати при потребі (для S2S reward і т.п.)
        LevelPlay.Init(appKey, null);
        Debug.Log("LevelPlay Init called with appKey: " + appKey);
    }

    #region Init Callbacks

    private void OnLevelPlayInitSuccess(LevelPlayConfiguration config)
    {
        Debug.Log("LevelPlay Init SUCCESS");

        //SetupRewardedAd();
    }

    #endregion

    #region Rewarded

    private void SetupRewardedAd()
    {
        
        rewardedAd = new LevelPlayRewardedAd(rewardedAdUnitId);

        // Підписуємося на події
        rewardedAd.OnAdLoaded += adInfo =>
        {
            Debug.Log("Rewarded Loaded: " + adInfo.AdUnitId);
        };

        rewardedAd.OnAdLoadFailed += error =>
        {
            Debug.LogError($"Rewarded Load Failed: {error.ErrorCode} - {error.ErrorMessage}");
        };

        rewardedAd.OnAdDisplayed += adInfo =>
        {
            Debug.Log("Rewarded Displayed");
        };

        rewardedAd.OnAdClosed += adInfo =>
        {
            Debug.Log("Rewarded Closed, перезавантажую…");
            rewardedAd.LoadAd(); // знову завантажуємо
        };

        rewardedAd.OnAdRewarded += (adInfo, reward) =>
        {
            Debug.Log($"Rewarded Reward! name={reward.Name}, amount={reward.Amount}");
            GiveRewardToPlayer(reward);
        };

        // Перше завантаження
        rewardedAd.LoadAd();
    }

    private void GiveRewardToPlayer(LevelPlayReward reward)
    {
        // TODO: тут даєш гравцю нагороду (монети, гем, енергію і т.д.)
        // приклад:
        // PlayerCoins += reward.Amount;
    }

    public void ShowRewarded()
    {
        if (rewardedAd != null && rewardedAd.IsAdReady())
        {
            rewardedAd.ShowAd(); // можна передати placementName, якщо треба
        }
        else
        {
            Debug.Log("Rewarded not ready, пробую перезавантажити");
            rewardedAd?.LoadAd();
        }
    }

    #endregion

   
}
