using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemGroup : MonoBehaviour
{
    [SerializeField]
    Image image;
    private MapType mapType;
    public void MoveTaget(Vector3 taget, float time, Action callback = null)
    {
        transform.DOMove(taget, time).OnComplete(() =>
        {
            if (callback != null)
                callback();
            this.gameObject.SetActive(false);
        });
    }
    public void TweenGroup(Color color, float time, Action callback = null)
    {
        this.image.transform.localScale = new Vector3(1, 2, 1);
        this.image.DOColor(color, time).OnComplete(() =>
        {
        });
        this.image.transform.DOScale(new Vector3(1,1.22f,1), time).SetEase(Ease.InSine).OnComplete(() =>
        {
            if (callback != null)
                callback();
            this.gameObject.SetActive(false);
        });
      
    }
    public void SetData(long amount)
    {
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
            this.image.color = Map.Instance.GetColor(amount);
            this.gameObject.SetActive(true);
        }
    }
}
