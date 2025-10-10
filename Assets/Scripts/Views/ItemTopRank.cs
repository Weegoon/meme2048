using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemTopRank : MonoBehaviour
{
    [SerializeField]
    TMP_Text txtScore;
   public void SetData(long score)
    {
        txtScore.text = Utils.FormatNumber1(score);
    }
}
