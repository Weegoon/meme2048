using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupShop : BasePopup
{
    [SerializeField]
    TMP_Text txtCoin;
    [SerializeField]
    Button btnExit;
    [SerializeField]
    GameObject Container;
    private void Awake()
    {
        btnExit.onClick.AddListener(OnExit);
        ReSize();
    }
    private void Start()
    {
        GameManager.Register(Event_e.DiamonChange, OnCoinChange);
    }
    private void ReSize()
    {
        var aspect = 1f / Camera.main.aspect;
        var scale = 1.7778f / aspect;
        if (aspect > 1.7778f)
        {
            Container.transform.localScale = new Vector3(scale, scale, 1);
        }
    }
    public void Onbuy600()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        Bridge.instance.BuyPack(() =>
        {
            GameManager.ChangeDiamond(600);
        }, GameConfig.Item_Coin_600);
    }
    public void Onbuy1600()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        Bridge.instance.BuyPack(() =>
        {
            GameManager.ChangeDiamond(1600);
        }, GameConfig.Item_Coin_1600);
    }
    public void Onbuy3400()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        Bridge.instance.BuyPack(() =>
        {
            GameManager.ChangeDiamond(3400);
        }, GameConfig.Item_Coin_3400);
    }
    public void Onbuy7200()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        Bridge.instance.BuyPack(() =>
        {
            GameManager.ChangeDiamond(7200);
        }, GameConfig.Item_Coin_7200);
    }
    public void Onbuy12000()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        Bridge.instance.BuyPack(() =>
        {
            GameManager.ChangeDiamond(12000);
        }, GameConfig.Item_Coin_12000);
    }
    public void Onbuy48000()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        Bridge.instance.BuyPack(() =>
        {
            GameManager.ChangeDiamond(48000);
        }, GameConfig.Item_Coin_48000);
    }

    public void RemoveAds()
    {
        //SoundManager.Instance.PlaySound("sfx_ui_select");
        //GameManager.Instance.RemoveAds(() =>
        //{
        //    btnRemoveAds.gameObject.SetActive(false);
        //});
    }
    public void ViewAds()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        Bridge.instance.ShowReward(() =>
        {
            GameManager.ChangeDiamond(GameConfig.Reward_Diamond);
        });
    }


    private void OnExit()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        base.OnClose();
    }
    public void Show()
    {
        SoundManager.Instance.PlaySound("sfx_ui_shop");
        //btnRemoveAds.gameObject.SetActive(!LocalStore.IsRemoveAds());
        OnCoinChange(LocalStore.GetDiamond());
        base.Show(Container);
    }
    private void OnCoinChange(object obj)
    {
        txtCoin.text = Utils.FormatNumber(System.Convert.ToInt64(obj));
    }
    private void OnDestroy()
    {
        GameManager.UnRegister(Event_e.DiamonChange, OnCoinChange);
    }
}
