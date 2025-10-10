using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupSummary : MonoBehaviour
{
    [SerializeField]
    TMP_Text txtScore, txtTime, txt2, txt4, txt8, txt16, txt32, txt64, txtSum;
    [SerializeField]
    Button buttonReplay;
    private Action callback;
    private void Awake()
    {
        buttonReplay.onClick.AddListener(()=> 
        {
            if (callback != null)
                callback();
            this.gameObject.SetActive(false);
        });
    }
    public void Show(Action callback,int score, int time, int count_2, int count_4, int count_8, int count_16, int count_32, int count_64)
    {
        this.callback = callback;
        txtScore.text = score + "";
        txtTime.text = Utils.TimeString(time);
        txt2.text = count_2 + "";
        txt4.text = count_4 + "";
        txt8.text = count_8 + "";
        txt16.text = count_16 + "";
        txt32.text = count_32 + "";
        txt64.text = count_64 + "";
        txtSum.text = (count_2 + count_4 + count_8 + count_16 + count_32 + count_64) + "";
        this.gameObject.SetActive(true);
    }
}
