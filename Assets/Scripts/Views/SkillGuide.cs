using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Spine;
using Spine.Unity;
using DG.Tweening;

public class SkillGuide : MonoBehaviour
{
    [SerializeField]
    TMP_Text txtTitle;
    [SerializeField]
    Button btnExit;
    Action<SkillType> callBack;
    SkillType skillType;
    [SerializeField]
    SkeletonGraphic skeletonDrop, skeletonSwap, skeletonDropEffect;
    [SerializeField]
    Image TitleDrop, TitleSwap;
    [SerializeField]
    GameObject Container;
    private void Awake()
    {
        ReSize();
        btnExit.onClick.AddListener(OnExit);
        DropPos = skeletonDrop.transform.position;
    }
    private void ReSize()
    {
        var aspect = 1f / Camera.main.aspect;
        var scale = 2f / aspect;
        if (aspect > 2f)
        {
            this.skeletonSwap.transform.localScale = new Vector3(scale, scale, 1);
            this.skeletonDrop.transform.localScale = new Vector3(scale, scale, 1);
            this.Container.transform.localScale = new Vector3(scale, scale, 1);
        }
    }
    private void OnExit()
    {
        if (this.callBack != null)
            this.callBack(this.skillType);
        TitleDrop.gameObject.SetActive(false);
        TitleSwap.gameObject.SetActive(false);
        skeletonDrop.gameObject.SetActive(false);
        skeletonSwap.gameObject.SetActive(false);
        skeletonDropEffect.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void EffectDropBroken(ItemPlay item, Action callbackEvent, Action callback)
    {
        this.callbackDropEvent = callbackEvent;
        this.callbackDropComplete = callback;
        //skeletonDropEffect = Instantiate(skeletonDropEffectTp);
        skeletonDropEffect.transform.position = item.transform.position;
        skeletonDropEffect.material.color = Map.Instance.GetColor(item.Amount);
        skeletonDrop.transform.DOMove(item.transform.position, 0.2f).OnComplete(() =>
        {
            isBroken = true;
            //Debug.LogError("skeletonDrop.AnimationState: "+ skeletonDrop.AnimationState);
            SoundManager.Instance.PlaySound("sfx_item_break");
            skeletonDrop.AnimationState.SetAnimation(0, "broken", false);
        });
    }

    private void Event1(TrackEntry trackEntry, Spine.Event e)
    {
        if (callbackDropEvent != null)
            callbackDropEvent();
        skeletonDropEffect.gameObject.SetActive(true);
        skeletonDropEffect.AnimationState.SetAnimation(0, "animation", false);
        skeletonDropEffect.AnimationState.Complete += DropCompleteEffect;
    }

    public void EffectSwapBroken(Vector3 position0, Vector3 position1, Action callback)
    {
        this.callbackSwap = callback;
        XoayComplete();
    }


    public void Show(SkillType skillType, Action<SkillType> callBack)
    {
        if (skillType == SkillType.Drop)
        {
            skeletonDrop.gameObject.SetActive(true);
            skeletonDrop.AnimationState.SetAnimation(0, "click", false);
            skeletonDrop.AnimationState.Complete += DropComplete;
            skeletonDrop.AnimationState.Event += Event1;
            TitleDrop.gameObject.SetActive(true);
        }
        else if (skillType == SkillType.Swap)
        {
            skeletonSwap.gameObject.SetActive(true);
            skeletonSwap.AnimationState.SetAnimation(0, "click", false);
            skeletonSwap.AnimationState.Complete += SwapComplete;
            TitleSwap.gameObject.SetActive(true);
        }
        this.skillType = skillType;
        txtTitle.text = skillType.ToString();
        this.callBack = callBack;
        this.gameObject.SetActive(true);
    }
    private void DropCompleteEffect(TrackEntry trackEntry)
    {

        if (trackEntry.Animation.Name == "animation")
        {
            skeletonDropEffect.AnimationState.SetAnimation(0, "empty", false);
            skeletonDropEffect.gameObject.SetActive(false);
            skeletonDropEffect.AnimationState.Complete -= DropCompleteEffect;
            if (this.callbackDropComplete != null)
                this.callbackDropComplete();
            this.gameObject.SetActive(false);
        }

    }
    private void OnDisable()
    {
        skeletonDrop.AnimationState.Event -= Event1;
        skeletonDrop.AnimationState.Complete -= DropComplete;
    }
    private void DropComplete(TrackEntry trackEntry)
    {
        if (trackEntry.Animation.Name == "click" && !isBroken)//skeletonDrop.AnimationState.ToString().Equals("click"))
        {
            skeletonDrop.AnimationState.SetAnimation(0, "click-idle", true);
        }
        if (trackEntry.Animation.Name.Equals("broken"))
        {
            isBroken = false;
            TitleDrop.gameObject.SetActive(false);
            skeletonDrop.gameObject.SetActive(false);
            skeletonDrop.gameObject.transform.position = DropPos;
            skeletonDrop.AnimationState.Event -= Event1;
            skeletonDrop.AnimationState.Complete -= DropComplete;

        }
    }
    private void XoayComplete()
    {
        skeletonSwap.gameObject.SetActive(false);
        TitleSwap.gameObject.SetActive(false);
        if (this.callbackSwap != null)
        {
            callbackSwap();
            this.gameObject.SetActive(false);
        }
    }
    private void SwapComplete(TrackEntry trackEntry)
    {
        if (trackEntry.Animation.Name == "click")
        {
            skeletonSwap.AnimationState.SetAnimation(0, "click-idle", true);
        }
    }
    private Action callbackDropEvent, callbackDropComplete, callbackSwap;
    Vector3 DropPos;
    bool isBroken = false;
}
