using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTemplate : MonoBehaviour
{
    [SerializeField]
    TMP_Text txtAmount;
    [SerializeField]
    Image image;
    [HideInInspector]
    public string Key;
    [HideInInspector]
    public long Amount;
    [HideInInspector]
    public int Row, Col;
    private MapType mapType;
    public void Init(MapType mapType, string key)
    {
        this.Key = key;
        this.Row = System.Convert.ToInt32(key.Split('-')[0]);
        this.Col = System.Convert.ToInt32(key.Split('-')[1]);
        this.mapType = mapType;
        SetData(0);
    }
    public void SetData(long amount)
    {
        this.Amount = amount;
        txtAmount.text = amount + "";
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
            this.image.sprite = Map.Instance.GetSprite(amount);
            this.gameObject.SetActive(true);
        }
    }

}
