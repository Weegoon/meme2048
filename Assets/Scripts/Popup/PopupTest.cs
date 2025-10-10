using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PopupTest : MonoBehaviour
{

    [SerializeField]
    TMP_InputField InputField2, InputField4, InputField8, InputField16, InputField32, InputField64;

    [SerializeField]
    TMP_Text txtError;
    [SerializeField]
    Button button;
    private Action<List<float>> callback;
    private void Awake()
    {
        txtError.text = "";
        button.onClick.AddListener(() =>
        {
            if (Validate() && this.callback != null)
            {
                callback(result);
                this.gameObject.SetActive(false);
            }

        });
    }

    public void Show(Action<List<float>> callback, List<float> list)
    {
        this.callback = callback;
        InputField2.text = list[0] + "";
        InputField4.text = list[1] + "";
        InputField8.text = list[2] + "";
        InputField16.text = list[3] + "";
        InputField32.text = list[4] + "";
        InputField64.text = list[5] + "";
        this.gameObject.SetActive(true);
    }

    private bool Validate()
    {
        if (string.IsNullOrEmpty(InputField2.text))
            rate_2 = 0;
        else
            rate_2 = (float)System.Convert.ToDouble(InputField2.text);

        if (string.IsNullOrEmpty(InputField4.text))
            rate_4 = 0;
        else
            rate_4 = (float)System.Convert.ToDouble(InputField4.text);

        if (string.IsNullOrEmpty(InputField8.text))
            rate_8 = 0;
        else
            rate_8 = (float)System.Convert.ToDouble(InputField8.text);

        if (string.IsNullOrEmpty(InputField16.text))
            rate_16 = 0;
        else
            rate_16 = (float)System.Convert.ToDouble(InputField16.text);

        if (string.IsNullOrEmpty(InputField32.text))
            rate_32 = 0;
        else
            rate_32 = (float)System.Convert.ToDouble(InputField32.text);

        if (string.IsNullOrEmpty(InputField64.text))
            rate_64 = 0;
        else
            rate_64 = (float)System.Convert.ToDouble(InputField64.text);

        if (rate_2 + rate_4 + rate_8 + rate_16 + rate_32 + rate_64 > 100)
        {
            txtError.text = "Tổng vượt quá 100%";
            return false;
        }
        else
        {
            result = new List<float>() { rate_2, rate_4, rate_8, rate_16, rate_32, rate_64 };
            txtError.text = "";
            return true;
        }

    }
    List<float> result = new List<float>();
    float rate_2, rate_4, rate_8, rate_16, rate_32, rate_64;
}
