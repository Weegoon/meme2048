using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePopup : MonoBehaviour
{
    Action AtOnclose;
    float Time = 0.5f;
    GameObject container;
    public virtual void Show(GameObject container, Action onclose = null)
    {
        this.container = container;
        this.AtOnclose = onclose;
        this.transform.position = Vector3.zero;
        this.container.transform.position = new Vector3(0f, 11f, 0);
        this.gameObject.SetActive(true);
        this.container.transform.DOMove(Vector3.zero, Time).SetEase(Ease.OutCubic);
    }
    public virtual void OnClose()
    {
        if (AtOnclose != null)
            AtOnclose();
        this.gameObject.SetActive(false);
        //this.transform.DOMove(new Vector3(0f, 11f, 0), Time).SetEase(Ease.OutCubic).OnComplete(() =>
        //{
        //    if (AtOnclose != null)
        //        AtOnclose();
        //    this.gameObject.SetActive(false);
        //});
    }
}
