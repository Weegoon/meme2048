using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemPlay : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    TMP_Text txtAmount;
    [SerializeField]
    Image image, imageDiamond, imageTick, imageKing;
    [HideInInspector]
    public string Key;
    [HideInInspector]
    public long Amount;
    [HideInInspector]
    public bool IsDiamond;
    [HideInInspector]
    public int Row, Col;
    [SerializeField]
    Image[] AmountImages;
    [SerializeField]
   GameObject AmountContainer;
    private MapType mapType;
    Action<string> callback;
    [SerializeField]
    ParticleSystem move_finish,star_light;
    public void Init(MapType mapType, string key, int amount, Action<string> callback)
    {
        this.Amount = amount;
        this.Key = key;
        this.callback = callback;
        this.Row = System.Convert.ToInt32(key.Split('-')[0]);
        this.Col = System.Convert.ToInt32(key.Split('-')[1]);
        this.mapType = mapType;
        //OffAllParticle();
        Diamond(false);
        SetData(this.Amount);
    }
    public Vector3 GetImpactPostion()
    {
        return move_finish.gameObject.transform.position;
    }
    //private void OnEnable()
    //{
    //    SetData(2048);
    //}
    public void ChangeMaxBlock(long MaxBlock)
    {
        if (this.Amount >= MaxBlock)
        {
            star_light.gameObject.SetActive(true);
            imageKing.gameObject.SetActive(true);
            star_light.Play();
        }
        else
        {
            star_light.gameObject.SetActive(false);
            imageKing.gameObject.SetActive(false);
            star_light.Stop();
        }
    }
    public void SetData(long amount)
    {
        Tick(false);
        this.Amount = amount;
        string strAmount= Utils.FormatNumber1(amount);
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
        ChangeAmount(this.Amount);
    }
    public void Diamond(bool value)
    {
        IsDiamond = value;
        imageDiamond.gameObject.SetActive(value);
    }
    private void ChangeAmount(long amount)
    {
        var MaxBlock = LocalStore.GetMaxBlock();
        //Debug.LogError("ChangeAmount: "+ amount+ " MaxBlock: "+ MaxBlock);
        if (amount <= 0)
        {
            Diamond(false);
            //OffAllParticle();
            this.gameObject.SetActive(false);
        }
        else
        {
            this.image.sprite = Map.Instance.GetSprite(amount);
            this.gameObject.SetActive(true);
        }
        if (amount>= MaxBlock)
        {
            star_light.gameObject.SetActive(true);
            imageKing.gameObject.SetActive(true);
            star_light.Play();
        }
        else
        {
            star_light.gameObject.SetActive(false);
            imageKing.gameObject.SetActive(false);
            star_light.Stop();
        }
    }
    private void SetText(string strAmount)
    {
        AmountContainer.transform.localScale = GetScale(strAmount.Length);
       for (int i=0;i< AmountImages.Length; i++)
        {
            if(i< strAmount.Length)
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
       switch(leght)
        {
            case 3:
                return new Vector3(0.89189f, 0.89189f, 0.89189f);
            case 4:
                return new Vector3(0.8378f, 0.8378f, 0.8378f);
            default:
                return Vector3.one;
        }    
    }    

    public void Tick(bool value)
    {
        imageTick.gameObject.SetActive(value);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.callback != null)
            this.callback(this.Key);
    }
}
