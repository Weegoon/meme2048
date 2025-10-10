using UnityEngine;
using UnityEngine.Events;
using System.Runtime.InteropServices;

public class XiaomiServices : MonoBehaviour
{
    public static XiaomiServices instance;

    [DllImport("__Internal")]
    public static extern void XiaomiSDK_ShowInterstitial();

    [DllImport("__Internal")]
    public static extern void XiaomiSDK_ShowFirstInterstitial();

    [DllImport("__Internal")]
    public static extern void XiaomiSDK_ShowReward();

    [DllImport("__Internal")]
    public static extern void XiaomiSDK_LoadReady();

    UnityAction rewardAdsSuccessEvent;

    void Awake()
    {
        instance = this;
    }

    public void OnResumeGame()
    {
        Debug.Log("Game Resume");
        Time.timeScale = 1f;
        SoundManager.Instance.UnmuteSound();
    }

    public void OnPauseGame()
    {
        Debug.Log("Game Pause");
        Time.timeScale = 0f;
        SoundManager.Instance.MuteSound();
    }

    public void OnGameReady()
    {
        XiaomiSDK_LoadReady();
    }

    public void OnRewardedGame()
    {
        Debug.Log("Granted Reward");
        if (this.rewardAdsSuccessEvent != null)
            rewardAdsSuccessEvent.Invoke();
    }

    public void OnPreloadRewardedVideo(int loaded)
    {
        if (loaded == 0)
            Debug.Log("SDK couldn't preload ad");
        else
            Debug.Log("SDK preloaded ad");
    }

    void OnRewardedVideoSuccess()
    {
        Debug.Log("Rewarded video success");
    }

    void OnRewardedVideoFailure()
    {
        Debug.Log("Rewarded video failure");
    }

    #region Interstitial Ads

    public void ShowInterstitialAd()
    {
        Debug.Log("Showing Interstitial Ad...");

        XiaomiSDK_ShowInterstitial();
    }

    public void ShowFirstInterstitialAd()
    {
        Debug.Log("Showing First Interstitial Ad...");

        XiaomiSDK_ShowFirstInterstitial();
    }

    #endregion

    #region Rewarded Video Ads

    public void ShowRewardedVideoAd(UnityAction onCompleted)
    {
        Debug.Log("Showing Rewarded Video Ad...");

        this.rewardAdsSuccessEvent = onCompleted;
        XiaomiSDK_ShowReward();
    }


    #endregion

    #region Banner Ads

    public void LoadBannerAd()
    {

    }

    #endregion

    #region Log Event

    #endregion
}
