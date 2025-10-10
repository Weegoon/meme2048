using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkLineMove : MonoBehaviour
{
    private LineRenderer _line;
    private void Start()
    {
        _line = GetComponent<LineRenderer>();
        //_line.startWidth = 0.03f;
        //_line.endWidth = 0.03f;
    }
    // Update is called once per frame
    void Update()
    {
        _line.material.SetTextureOffset("_MainTex", Vector2.left * Time.time);
    }
}
