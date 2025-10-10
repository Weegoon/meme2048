using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonEffect : MonoBehaviour,IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        OnButtonClicked();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       //
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       //
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //
    }
    private void OnButtonClicked()
    {
        this.transform.DOScale(new Vector3(1.3f,1.3f,1.3f),0.12f).OnComplete(()=> 
        {
            this.transform.DOScale(Vector3.one, 0.12f);
        });
    }
}
