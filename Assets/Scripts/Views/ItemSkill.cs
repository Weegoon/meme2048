using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ItemSkill : MonoBehaviour
{
    public SkillType skillType;
    [SerializeField]
    TMP_Text txtCount, txtPrice;
    Button button;
    [SerializeField]
    GameObject goCount, goDiamond;
    private bool isUse;
    // Start is called before the first frame update
    private Action<SkillType> OnUser, OnBuy;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (!isUse)
        {
            SoundManager.Instance.PlaySound("sfx_ui_item");
            if (this.count > 0)
            {
                if (OnUser != null)
                    OnUser(skillType);
            }
            else
            {
                if (OnBuy != null)
                    OnBuy(skillType);
            }
        }
    }
    public void SetData(int count, int price)
    {
        isUse = false;
        this.count = count;
        this.price = price;
        txtCount.text = Utils.FormatNumber1(count);
        txtPrice.text = Utils.FormatNumber1(price);
        if (this.count > 0)
        {
            goCount.gameObject.SetActive(true);
            goDiamond.gameObject.SetActive(false);
        }
        else
        {
            goCount.gameObject.SetActive(false);
            goDiamond.gameObject.SetActive(true);
        }

    }
    public void Init(int count, int price, Action<SkillType> onUser, Action<SkillType> onBuy)
    {
        isUse = false;
        this.count = count;
        this.price = price;
        txtCount.text = this.count + "";
        txtPrice.text = this.price + "";
        this.OnUser = onUser;
        this.OnBuy = onBuy;
        if (this.count > 0)
        {
            goCount.gameObject.SetActive(true);
            goDiamond.gameObject.SetActive(false);
        }
        else
        {
            goCount.gameObject.SetActive(false);
            goDiamond.gameObject.SetActive(true);
        }

    }
    private int count, price;
}
