using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemMove : MonoBehaviour
{
    [SerializeField]
    TMP_Text txtAmount;
    [SerializeField]
    Image /*image,*/ trail;
    [SerializeField]
    Image[] AmountImages;
    [SerializeField]
    GameObject AmountContainer;
    private MapType mapType;
    public void MoveTaget(Vector3 taget, float time, Action callback = null)
    {
        transform.DOMove(taget, time).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            if (callback != null)
                callback();
            this.gameObject.SetActive(false);
        });
    }
    public void SetData(long amount, bool activeTrail = false)
    {
        trail.gameObject.SetActive(activeTrail);
        string strAmount = Utils.FormatNumber1(amount);
        SetText(strAmount);
        //if (strAmount.Length <= 2)
        //    txtAmount.fontSize = 100;
        //else if (strAmount.Length == 3)
        //{
        //    txtAmount.fontSize = 80;
        //}
        //else
        //    txtAmount.fontSize = 70;
        //txtAmount.text = strAmount;
        ChangeAmount(amount);
    }
    private void ChangeAmount(long amount)
    {
        if (amount <= 0)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            //this.image.sprite = Map.Instance.GetSprite(amount);
            this.trail.color = Map.Instance.GetColor(amount);
            this.gameObject.SetActive(true);
        }
    }
    private void SetText(string strAmount)
    {
        AmountContainer.transform.localScale = GetScale(strAmount.Length);
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

    private Vector3 GetScale(int leght)
    {
        switch (leght)
        {
            case 3:
                return new Vector3(0.89189f, 0.89189f, 0.89189f);
            case 4:
                return new Vector3(0.8378f, 0.8378f, 0.8378f);
            default:
                return Vector3.one;
        }
    }
}
