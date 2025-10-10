using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Spine.Unity;
using Spine;
using DG.Tweening;

public class PopupNewBlock : BasePopup
{
    [SerializeField]
    Button button;
    [SerializeField]
    TMP_Text txtDiamond;
    [SerializeField]
    ItemMove ItemOld, ItemNewBlock, ItemAddLock, ItemRemove, ItemLock, itemMove;
    [SerializeField]
    GameObject Container;
    [SerializeField]
    SkeletonGraphic mySkeletonAnimation;
    private void Awake()
    {
        button.onClick.AddListener(OnClose);
        //mySkeletonAnimation.material.color=Color.red;
    }

    private void OnClose()
    {
        SoundManager.Instance.PlaySound("sfx_ui_select");
        base.OnClose();
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
    private void OnEnable()
    {
        //Show(2, 2048);
    }
    public void Show(long remove, long newBlock)
    {
        this.amount = remove;
        mySkeletonAnimation.gameObject.SetActive(false);
        txtDiamond.text = Utils.FormatNumber(LocalStore.GetDiamond());
        ItemNewBlock.SetData(newBlock);
        ItemOld.SetData(newBlock / 2);
        ItemAddLock.SetData(newBlock * 2);
        ItemRemove.SetData(remove);
        ItemLock.SetData(remove * 2);
        Effect(this.amount);
        base.Show(Container);
    }

    void Effect(long amount)
    {
        ItemRemove.gameObject.SetActive(true);
        ItemRemove.transform.localScale = Vector3.one;
        ItemRemove.transform.DOScale(Vector3.one, 1f).OnComplete(() =>
        {
            ItemRemove.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.3f).SetEase(Ease.InBounce).OnComplete(() =>
            {
                SoundManager.Instance.PlaySound("sfx_new_block_number");
                mySkeletonAnimation.gameObject.SetActive(true);
                ItemRemove.gameObject.SetActive(false);
                mySkeletonAnimation.material.color = Map.Instance.GetColor(amount);
                mySkeletonAnimation.AnimationState.Complete += OnComplete;
                mySkeletonAnimation.AnimationState.SetAnimation(0, "animation", false);
            });
        });
    }
    private void OnComplete(TrackEntry trackEntry)
    {
        if (trackEntry.Animation.Name == "animation")
        {
            mySkeletonAnimation.AnimationState.SetAnimation(0, "empty", false);
            mySkeletonAnimation.gameObject.SetActive(false);
            mySkeletonAnimation.AnimationState.Complete -= OnComplete;
            OnTweenEffect();
        }
    }
    private void OnTweenEffect()
    {
        float time = 0.5f;
        ItemLock.gameObject.SetActive(false);
        itemMove.transform.position = ItemLock.transform.position;
        itemMove.SetData(this.amount * 2);
        itemMove.gameObject.SetActive(true);
        // //
        itemMove.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), time);
        itemMove.transform.DOMove(ItemRemove.transform.position, time).SetEase(Ease.OutQuint).OnComplete(() =>
        {
            ItemRemove.SetData(this.amount * 2);
            ItemRemove.gameObject.SetActive(true);
            itemMove.transform.position = new Vector3(ItemLock.transform.position.x + 10, ItemLock.transform.position.y, ItemLock.transform.position.z);
            itemMove.SetData(this.amount * 4);
            // //
            itemMove.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), time);
            itemMove.transform.DOMove(ItemLock.transform.position, time).SetEase(Ease.OutQuint).OnComplete(() =>
            {
                ItemLock.SetData(this.amount * 4);
                ItemLock.gameObject.SetActive(true);
                itemMove.gameObject.SetActive(false);
                itemMove.transform.localScale = Vector3.one;
            });
        });
       
    }
    private long amount;
}
