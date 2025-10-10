using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupRate : BasePopup
{
    [SerializeField]
    Button buttonRate,buttonExit;
    [SerializeField]
    GameObject Container;
    private void Awake()
    {
        buttonRate.onClick.AddListener(OnRate);
        buttonExit.onClick.AddListener(OnClose);
    }
    private void Start()
    {
        ReSize();
    }
    private void ReSize()
    {
        var aspect = 1f / Camera.main.aspect;
        var scale = 2f / aspect;
        if (aspect > 2f)
        {
            Container.transform.localScale = new Vector3(scale, scale, 1);
        }
    }
    private void OnRate()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        Bridge.instance.OpenRate();
        LocalStore.SetInt(LocalStore.LS_Rate, 1);
        base.OnClose();
    }
    private void OnClose()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        base.OnClose();
    }    
    public void Show()
    {
        base.Show(Container);
    }
}
