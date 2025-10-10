using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class PopupSetting : BasePopup
{
    [SerializeField]
    TMP_Text txtDiamond;
    [SerializeField]
    Button btnRank, btnReplay, btnHome, btnExit;
    [SerializeField]
    Toggle toggleSound, toggleVibrate;
    [SerializeField]
    GameObject Container;
    private void Awake()
    {
        btnRank.onClick.AddListener(OnRanking);
        toggleSound.onValueChanged.AddListener((value) => OnMusic(value));
        toggleVibrate.onValueChanged.AddListener((value) => OnVibrate(value));
        btnReplay.onClick.AddListener(OnReplay);
        btnHome.onClick.AddListener(OnHome);
        btnExit.onClick.AddListener(OnExit);
    }
    private void Start()
    {
        toggleSound.isOn = LocalStore.GetSound();
        toggleVibrate.isOn = LocalStore.GetVibrate();
        GameManager.Register(Event_e.DiamonChange, OnDiamondChange);
    }
    private void OnExit()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        base.OnClose();
    }

    private void OnRanking()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        Map.Instance.TopRank();
    }

    private void OnMusic(bool open)
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        SoundManager.Instance.OpenSound(open);
    }

    private void OnVibrate(bool open)
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        if (open)
            //Handheld.Vibrate();
        GameManager.Instance.OpenVibrate(open);
    }

    private void OnReplay()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        Map.Instance.Replay();
        this.gameObject.SetActive(false);
    }

    public void OnNewGame()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        Map.Instance.Replay();
        this.gameObject.SetActive(false);
    }

    private void OnHome()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        SceneManager.LoadSceneAsync(1);
    }

    private void OnRemoveAds()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
    }

    public void Show()
    {
        OnDiamondChange(LocalStore.GetDiamond());
        base.Show(Container);
    }

    private void OnDiamondChange(object obj)
    {
        txtDiamond.text = Utils.FormatNumber((int)obj);
    }
    private void OnDestroy()
    {
        GameManager.UnRegister(Event_e.DiamonChange, OnDiamondChange);
    }
}
