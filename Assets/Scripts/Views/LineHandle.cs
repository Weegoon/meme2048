using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LineHandle : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    Image image;
    [HideInInspector]
    public int Col;
    private Action<int> PointerClick;
    Color nomalColor = Color.white;
    [SerializeField]
    Color activecolor;
    private void Awake()
    {
        nomalColor = image.color;
    }

    public void InitLine(int col, Action<int> pointerClick)
    {
        this.Col = col;
        this.PointerClick = pointerClick;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (PointerClick != null)
            PointerClick(this.Col);
    }

    public void ActiveLine(bool active)
    {
        image.color = active ? activecolor : nomalColor;
    }

}
