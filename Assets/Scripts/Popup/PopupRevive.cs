using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupRevive : BasePopup
{
    [SerializeField]
    Button btnRevive, btnReviveFree, btnExit;
    [SerializeField]
    TMP_Text txtDiamond, txtprice;
    Action<bool> callBack;
    [HideInInspector]
    int price;
    [SerializeField]
    GameObject Container, ReesSize, Heart;
    [SerializeField]
    ParticleSystem lightheart;
    private void Awake()
    {
        btnRevive.onClick.AddListener(OnRevive);
        btnReviveFree.onClick.AddListener(OnReviveFree);
        btnExit.onClick.AddListener(OnClose);
    }
    private void Start()
    {
        GameManager.Register(Event_e.DiamonChange, OnDiamondChange);
        ReSize();
    }

    private void OnDiamondChange(object obj)
    {
       var diamond = (int)obj;
        txtDiamond.text = Utils.FormatNumber(diamond);
    }

    private void ReSize()
    {
        var aspect = 1f / Camera.main.aspect;
        var scale = 2f / aspect;
        if (aspect > 2f)
        {
            ReesSize.transform.localScale = new Vector3(scale, scale, 1);
            Heart.transform.localScale = new Vector3(0.9f, 0.9f, 1);
        }
    }
    private void OnClose()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        if (this.callBack != null)
            callBack(false);
        base.OnClose();
    }

    private void OnReviveFree()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        Bridge.instance.ShowReward(() =>
        {
            if (this.callBack != null)
                callBack(true);
            base.OnClose();
        });
    }

    private void OnRevive()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        var diamond = LocalStore.GetDiamond();
        if (diamond > this.price)
        {
            GameManager.ChangeDiamond(-this.price);
            if (this.callBack != null)
                callBack(true);
            base.OnClose();
        }
        else
        {
            Map.Instance.Shop();
        }
    }
    public void Show(int price, Action<bool> callBack)
    {
        SoundManager.Instance.PlaySound("sfx_revive");
        this.callBack = callBack;
        lightheart.Play();
        this.txtDiamond.text = Utils.FormatNumber(LocalStore.GetDiamond());
        this.price = price;
        if (this.price < 500)
        {
            btnRevive.transform.position = new Vector3(-btnReviveFree.transform.position.x, btnReviveFree.transform.position.y, btnReviveFree.transform.position.z);
            btnReviveFree.gameObject.SetActive(true);
        }    
        else
        {
            btnRevive.transform.position = new Vector3(0, btnReviveFree.transform.position.y, btnReviveFree.transform.position.z);
            btnReviveFree.gameObject.SetActive(false);
        }    
        txtprice.text = "" + this.price;
        base.Show(Container);
    }
    private void OnDestroy()
    {
        GameManager.UnRegister(Event_e.DiamonChange, OnDiamondChange);
    }
}
