using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    Button button;
    [SerializeField]
    SkeletonGraphic skeletonHand;
    [SerializeField]
    Image ImageGoodjob;
    Action callback;
    private void Awake()
    {
        button.onClick.AddListener(OnClicked);
        ReSize();
    }

    private void OnClicked()
    {
        if (this.callback != null)
            this.callback();
    }
    private void ReSize()
    {
       var aspect = 1f / Camera.main.aspect;
       var scale = 1.7778f / aspect;
        //if (aspect > 2f)
        //{
        //    this.Container.transform.localScale = new Vector3(scale, scale, 1);
        //    this.ItemTemplate.transform.localScale = new Vector3(scale, scale, 1);
        //}
        if (aspect > 1.7778f)
        {
            scale = 1.7778f / aspect;
            this.skeletonHand.transform.localScale = new Vector3(scale, scale, 1);
        }
        else
        {
            scale = 0.95f;
            this.skeletonHand.transform.localScale = new Vector3(scale, scale, 1);
        }

    }
    public void Show(ItemPlay item, Action callback)
    {
        this.callback = callback;
        button.gameObject.SetActive(true);
        skeletonHand.gameObject.SetActive(true);
        skeletonHand.transform.position = item.transform.position;
        button.transform.position = item.transform.position;
        //skeletonHand.AnimationState.SetAnimation(0, "tap-loop", true);
        this.gameObject.SetActive(true);
    }
    public void ChangeItem(ItemPlay item)
    {
        skeletonHand.transform.position = item.transform.position;
        button.transform.position = item.transform.position;
    }
    public void ShowGoodJob()
    {
        skeletonHand.gameObject.SetActive(false);
        button.gameObject.SetActive(false);
        ImageGoodjob.transform.localScale = Vector3.zero;
        ImageGoodjob.gameObject.SetActive(true);
        SoundManager.Instance.PlaySound("sfx_tutorial");
        ImageGoodjob.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            ImageGoodjob.transform.DOScale(Vector3.one, 1.5f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                this.gameObject.SetActive(false);
            });
        });
    }
}
