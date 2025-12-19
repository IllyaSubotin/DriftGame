public interface IAdService
{
    void Initialize();
    
    // Interstitial
    void LoadInterstitial();
    bool IsInterstitialReady();
    void ShowInterstitial();

    // Rewarded
    void LoadRewarded();
    bool IsRewardedReady();
    void ShowRewarded(System.Action onFinished);

    // Banner
    void LoadBanner();
    void ShowBanner();
    void HideBanner();
}
