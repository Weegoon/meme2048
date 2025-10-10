using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class MainView : MonoBehaviour
{
    [SerializeField]
    CrossAdsEvent crossAdsEvent;
    [SerializeField]
    TMP_Text txtDiamond, txtHighScore;
    [SerializeField]
    PopupShop popupShop;
    // Start is called before the first frame update
    [SerializeField]
    Button btnPlay, btnShop, btnViewVideo;
    [SerializeField]
    GameObject Container;
    [SerializeField]
    Image[] AmountImages;
    private void Awake()
    {
        btnPlay.onClick.AddListener(OnPlay);
        btnViewVideo.onClick.AddListener(OnViewAds);
        btnShop.onClick.AddListener(OnOpenShop);
    }
    private void Start()
    {
        ReSize();
        if (crossAdsEvent)
            crossAdsEvent.ShowCrossAds();
        txtHighScore.text = Utils.FormatNumber1(LocalStore.HighScore());
        GameManager.Register(Event_e.DiamonChange, OnDiamondChange);
        OnDiamondChange(LocalStore.GetDiamond());
        Bridge.instance.ShowBanner();
    }
    private void OnEnable()
    {
        GameManager.Instance.OpenSound(LocalStore.GetSound());
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
    public void OnOpenShop()
    {
        popupShop.Show();
    }
    private void OnViewAds()
    {
        Bridge.instance.ShowReward(() =>
        {
            GameManager.ChangeDiamond(GameConfig.Reward_Diamond);
        });
    }
    private void SetText(string strAmount)
    {
        for (int i = 0; i < AmountImages.Length; i++)
        {
            if (i < strAmount.Length)
            {
                AmountImages[i].sprite = Map.Instance.GetAmountSpriteByChar(strAmount[i]);
                AmountImages[i].gameObject.SetActive(true);
            }
            else
            {
                AmountImages[i].gameObject.SetActive(false);
            }
        }
    }
    private void OnRemoveAds()
    {
        //GameManager.Instance.RemoveAds(() =>
        //{
        //    btnRemoveAds.gameObject.SetActive(false);
        //});
    }



    private void OnPlay()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select_play");
        Bridge.instance.ShowInterstitial();
        SceneManager.LoadScene("GamePlay");
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
