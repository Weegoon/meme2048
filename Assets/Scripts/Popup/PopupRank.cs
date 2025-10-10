using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupRank : BasePopup
{
    [SerializeField]
    Button btnExit;
    [SerializeField]
    List<ItemTopRank> listItems;
    [SerializeField]
    GameObject Container;
    private void Awake()
    {
        btnExit.onClick.AddListener(OnExit);
    }
    private void Start()
    {
        ReSize();
    }
    private void OnExit()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        base.OnClose();
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
    public void Show()
    {
        var ranks = LocalStore.GetTopRank();
        for (int i = 0; i < listItems.Count; i++)
        {
            listItems[i].SetData(ranks[i]);
        }
        base.Show(Container);
    }
}
