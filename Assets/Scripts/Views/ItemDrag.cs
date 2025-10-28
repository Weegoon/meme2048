using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDrag : MonoBehaviour
{
    [SerializeField]
    TMP_Text txtAmount;
    [SerializeField]
    Image /*image,*/ imageDiamond;
    [SerializeField]
    Image[] AmountImages;
    [SerializeField]
    GameObject AmountContainer;
    [HideInInspector]
    public string Key;
    [HideInInspector]
    public long Amount;
    [HideInInspector]
    public bool IsDiamond;
    private MapType mapType;
    private Action PointerDown, PointerDrag, PointerUp;

    public GameObject ObjItem;

    public void InitDrag(Action PointerDown, Action PointerDrag, Action PointerUp)
    {
        this.PointerDown = PointerDown;
        this.PointerDrag = PointerDrag;
        this.PointerUp = PointerUp;
        Diamond(false);

        AmountContainer.SetActive(false);
    }
    public void SetData(long amount)
    {
        this.Amount = amount;
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
    public void Diamond(bool value)
    {
        IsDiamond = value;
        imageDiamond.gameObject.SetActive(value);
    }
    private void ChangeAmount(long amount)
    {
        if (amount <= 0)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            if (ObjItem != null)
            {
                Map.Instance.objectPoolManager.DespawnGameObject(ObjItem);
                ObjItem = null;
            }
            ObjItem = Map.Instance.SetMemePrefab(amount, this.transform);
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
    public void OnPointerDown()
    {
        Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = point;
        if (PointerDown != null)
        {
            PointerDown();
        }
    }
    public void OnPointerDrag()
    {
        Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = point;
        if (PointerDrag != null)
        {
            PointerDrag();
        }
    }
    public void OnPointerUp()
    {
        if (PointerUp != null)
        {
            PointerUp();
        }
    }
}
