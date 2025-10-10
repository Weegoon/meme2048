using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class PopupNewScore : BasePopup
{
    [SerializeField]
    TMP_Text txtDiamond, txtDiamondViewAds;
    [SerializeField]
    ItemMove ItemOld, ItemNewBlock, ItemAddLock;
    [SerializeField]
    Button btnViewAds, btnFree;
    [SerializeField]
    List<ItemRound> listItem;
    [SerializeField]
    Transform point;
    [SerializeField]
    GameObject start, end;
    // Start is called before the first frame update
    [SerializeField]
    GameObject Container;
    private void Awake()
    {
        btnViewAds.onClick.AddListener(OnViewAdd);
        btnFree.onClick.AddListener(OnFree);
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
    private void OnFree()
    {
        SoundManager.Instance.StopSound();
        SoundManager.Instance.PlaySound("sfx_ui_select");
        enableMove = false;
        GameManager.ChangeDiamond(GameConfig.Free_Diamond);
        base.OnClose();
    }

    private void OnViewAdd()
    {
        SoundManager.Instance.StopSound();
        SoundManager.Instance.PlaySound("sfx_ui_select");
        var x = point.transform.position.x;
        var rate = GetRate(x);
        enableMove = false;
        point.gameObject.SetActive(false);
        Bridge.instance.ShowReward(() => 
        {
            GameManager.ChangeDiamond(GameConfig.Free_Diamond * rate);
            base.OnClose();
        });
    }
    private void Update()
    {
        if (enableMove)
        {
            if (right)
            {
                point.transform.position += Vector3.right * 4 * Time.deltaTime;
                if (point.transform.position.x > end.transform.position.x)
                {
                    right = false;
                    point.transform.position = end.transform.position;
                }
            }
            else
            {
                point.transform.position -= Vector3.right * 4 * Time.deltaTime;
                if (point.transform.position.x < start.transform.position.x)
                {
                    right = true;
                    point.transform.position = start.transform.position;
                }
            }
        }
    }
    private void FixedUpdate()
    {
        var x = point.transform.position.x;
        var rate = GetRate(x);
        var diamond = GameConfig.Free_Diamond * rate;
        txtDiamondViewAds.text = "+" + Utils.FormatNumber(diamond);
    }
    private void OnEnable()
    {
        //Show(2,2048,()=>{ });
    }
    public void Show(long oldScore, long newScore, Action callback)
    {
        SoundManager.Instance.PlaySound("sfx_number_score");
        SoundManager.Instance.PlaySound("sfx_new_score_spin");
        txtDiamond.text = Utils.FormatNumber(LocalStore.GetDiamond());
        enableMove = true;
        right = true;
        ItemOld.SetData(oldScore);
        ItemNewBlock.SetData(newScore);
        ItemAddLock.SetData(newScore * 2);
        point.gameObject.SetActive(true);
        base.Show(Container, callback);
    }
    private void OnMove(Vector3 Taget)
    {
        point.transform.DOMove(Taget, 1f).OnComplete(() =>
        {
            if (enableMove)
            {
                if (Taget == end.transform.position)
                    OnMove(start.transform.position);
                else
                    OnMove(end.transform.position);
            }

        });
    }
    private int GetRate(float x)
    {
        ItemRound item = listItem[0];
        var disMin = Mathf.Abs(item.transform.position.x - x);
        foreach (ItemRound li in listItem)
        {
            var disTemp = Mathf.Abs(li.transform.position.x - x);
            if (disTemp < disMin)
            {
                disMin = disTemp;
                item = li;
            }
        }
        return item.rate;
    }
    private bool enableMove;
    private bool right;
}
