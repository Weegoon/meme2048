using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemDiamondEfect : MonoBehaviour
{
    [SerializeField]
    ParticleSystem particleSystem;  
    public void MoveTaget(Vector3 taget, float time, Action callback = null)
    {
        particleSystem.Play();
        //transform.DOScale(new Vector3(0.5f,0.5f), time);
        transform.DOMove(taget, time).OnComplete(() =>
        {
            if (callback != null)
                callback();
            Destroy(this.gameObject);
        });
    }
}
