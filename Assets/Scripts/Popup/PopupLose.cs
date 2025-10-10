
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupLose : BasePopup
{
    [SerializeField]
    TMP_Text txtDiamond, txtScore, txtHightScore;
    [SerializeField]
    Button buttonReplay, buttonShop, btnExit;
    [SerializeField]
    GameObject Container;
    private void Awake()
    {
        buttonReplay.onClick.AddListener(OnReplay);
        buttonShop.onClick.AddListener(OnShop);
        btnExit.onClick.AddListener(OnExit);
    }
    private void Start()
    {
        GameManager.Register(Event_e.DiamonChange, OnDiamondChange);
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
    private void OnDiamondChange(object obj)
    {
        txtDiamond.text = Utils.FormatNumber((int)obj);
    }
    private void OnExit()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        SceneManager.LoadSceneAsync(1);
    }

    public void Show(long score,long hightScore)
    {
        SoundManager.Instance.PlaySound("sfx_over");
        OnDiamondChange(LocalStore.GetDiamond());
        txtScore.text = Utils.FormatNumber1(score);
        txtHightScore.text = Utils.FormatNumber1(hightScore);
        base.Show(Container);
    }
    private void OnShop()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        Map.Instance.Shop();
    }

    private void OnShare()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        GameView.Instance.OnShare();
    }
    private void OnReplay()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        Map.Instance.Replay();
        base.OnClose();
    }
    private void OnDestroy()
    {
        GameManager.UnRegister(Event_e.DiamonChange, OnDiamondChange);
    }
}
