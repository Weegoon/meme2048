using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class Bridge : MonoBehaviour
{
    public static Bridge instance;

    public Queue<Action> ExecuteOnMainThread = new Queue<Action>();
    public Queue<Action> QueueFirebaseLogEvent = new Queue<Action>();

    // Use this for initialization
    void Awake()
    {
        Bridge[] mytypes = FindObjectsOfType(typeof(Bridge)) as Bridge[];
        if (mytypes.Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        while (ExecuteOnMainThread.Count > 0)
        {
            ExecuteOnMainThread.Dequeue().Invoke();
        }
        while (QueueFirebaseLogEvent.Count > 0)
        {
            QueueFirebaseLogEvent.Dequeue().Invoke();
        }
    }

    public void PurchaseItem(UnityAction e, string itemID)
    {

    }

    public void BuyPack(UnityAction e, string itemID)
    {
        PurchaseItem(e, itemID);
    }


    public void OpenRate()
    {

    }

    public void OpenInstagram()
    {
        Application.OpenURL("https://www.instagram.com/weegoonstudio/");
    }

    public void Facebook()
    {
        Application.OpenURL("https://www.facebook.com/Weegoon-337477433663758");
    }

    public void Youtube()
    {
        Application.OpenURL("https://www.youtube.com/channel/UCLwuF4EwRHbGQlLLWdomPeg/videos");
    }

    public void Twitter()
    {
        Application.OpenURL("https://twitter.com/WEEGOON1");
    }

    public void MoreGame()
    {
        Application.OpenURL("https://weegoon.vn/policy");
    }

    public void RestorePurchase()
    {
        Debug.Log("RestorePurchase");
    }

    public void RemoveAds(UnityAction e)
    {
        //PurchaseItem(e, ITEM_ID.NO_ADS);
    }

    public void ShowReward(UnityAction OnCompleted)
    {
        Debug.Log("Show Rewarded Ad");
#if XIAOMI_BUILD && !UNITY_EDITOR
        XiaomiServices.instance.ShowRewardedVideoAd(OnCompleted);
#else
        OnCompleted.Invoke();
#endif
    }

    public void OnGameReady()
    {
#if XIAOMI_BUILD && !UNITY_EDITOR
        XiaomiServices.instance.OnGameReady();
#endif
    }

    public void ShowInterstitial()
    {
        Debug.Log("ShowInterstitial");
#if XIAOMI_BUILD && !UNITY_EDITOR
        XiaomiServices.instance.ShowInterstitialAd();
#endif
    }

    public void ShowBanner()
    {
        Debug.Log("Show Banner");
    }
    public void HideBanner()
    {
        Debug.Log("Hide Banner");
    }

    public static List<Item> Items = new List<Item>()
    {
        new Item(){ itemID = ITEM_ID.NO_ADS, Price = 2.99f, PriceString = "$2.99"},
    };

    public void SendEvent(string eventName)
    {

    }

    public void SendEventLoadLevel(int lv)
    {
        string level = "0";
        if (lv < 10)
        {
            level = "0" + lv;
        }
        else
        {
            level = "" + lv;
        }
        Debug.Log("Load Level " + level);
    }


#if UNITY_IOS
    public void PauseGame()
    {
        SoundManager.Instance.Mute();
    }

    public void ResumeGame()
    {
        SoundManager.Instance.UnMute();
    }
#endif

    RectTransform g_container = null;
    public void ShowCrossAds(RectTransform container, float delayShow)
    {

    }

    public void HideCrossAds()
    {

    }
}

public class Item
{
    public string itemID;
    public int index;
    public float Price;
    public string PriceString;
    public double usdprice;
}

public class ITEM_ID
{
    public const string NO_ADS = "parkingjam_removeads";
}

public enum InterstitialPosition
{
    LoadLevel = 0, //Click vao choi 1 level trong man SelectLevel
    NextButtonClick = 1, //Click vao button next
    ReplayButtonClick = 2, //Click vao button replay
    ShowWinPopup = 3, //Luc popup win bat dau hien len
    ShowLose = 4, //Luc man lose bat dau hien len
    BackButtonClick = 5 //Click vao button back
}