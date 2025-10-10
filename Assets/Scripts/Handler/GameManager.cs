
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    private static Dictionary<Event_e, Action<object>> _handle = new Dictionary<Event_e, Action<object>>();
    [SerializeField]
    AudioSource audioSource;
    protected override void Awake()
    {
        Application.targetFrameRate = 60;
        base.Awake();
    }
    private static int diamond;
    private void Start()
    {
        IsVibrate = LocalStore.GetVibrate();
        diamond = LocalStore.GetDiamond();
        OpenSound(LocalStore.GetSound());
        PlaySound("background", true);
    }
    public static void ChangeDiamond(int value)
    {
        SoundManager.Instance.PlaySound("sfx_diamond");
        diamond += value;
        LocalStore.SetDiamond(diamond);
        Raise(Event_e.DiamonChange, diamond);
    }
    public static void SaveDiamond(int value)
    {
        LocalStore.SetDiamond(value);
        Raise(Event_e.DiamonChange, value);
    }
    public static void Register(Event_e e, Action<object> callback)
    {
        if (_handle.ContainsKey(e))
        {
            _handle[e] += callback;
        }
        else
        {
            _handle.Add(e, callback);
        }
    }
    public static void UnRegister(Event_e e, Action<object> callback)
    {
        if (_handle.ContainsKey(e))
            _handle[e] -= callback;
    }
    public static void Raise(Event_e e, object param)
    {
        if (_handle.ContainsKey(e))
        {
            _handle[e]?.Invoke(param);
        }
        else
        {
            Debug.LogError("Event " + e.ToString() + " litsener unregister");
        }
    }
    public void OpenVibrate(bool IsOpen)
    {
        IsVibrate = IsOpen;
        LocalStore.SetInt(LocalStore.LS_Vibrate, IsOpen ? 0 : 1);
    }
    public void PlaySound(string name, bool loop = false)
    {
        AudioClip clip = ResourcesCache.Load<AudioClip>(@"Sounds/" + name);
        if (clip != null)
        {
            if (loop)
            {
                audioSource.clip = clip;
                audioSource.loop = true;
                audioSource.Play();
            }
            else
            {
                audioSource.PlayOneShot(clip);
            }

        }
    }
    public void OpenSound(bool IsOpen)
    {
        audioSource.volume = IsOpen ? 1 : 0;
    }
    public void RemoveAds(Action RemoveAsdFinish)
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        this.RemoveAsdFinish = RemoveAsdFinish;
        Bridge.instance.RemoveAds(OnRemoveAdsCallback);
    }
    public void OnRemoveAdsCallback()
    {
        LocalStore.SetInt(LocalStore.LS_Remove_Ads, 1);
        if (RemoveAsdFinish != null)
            RemoveAsdFinish();
    }
    public void OnViewAdsCallback()
    {
        Debug.LogError("OnViewAdsCallback");
    }
    private void Show()
    {
        Bridge.instance.ShowReward(() =>
        {

        });

        Bridge.instance.ShowInterstitial();

        Bridge.instance.ShowBanner();
    }
    Action RemoveAsdFinish;
    public bool IsVibrate;
}
public enum Event_e
{
    DiamonChange,
    ExcuteSkillThrough,
    ExcuteSkillInvisible,
    AsyncSnakePositionOnMinimap
}