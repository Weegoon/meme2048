using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameView : MonoBehaviour
{
    public static GameView Instance;
    [SerializeField]
    TMP_Text txtFps;
    [SerializeField]
    ImageCapturing imageCapturing;
    private int count = 0;
    private void Awake()
    {
        Instance = this;
    }
    public void OnShare()
    {
        imageCapturing.ScreenshotAndShare();
    }    
    private void Start()
    {
        GameManager.Instance.OpenSound(false);
    }
    private void Update()
    {
        //count++;
    }
    private void SetFPS()
    {
        //txtFps.text = "FPS: " + count;
        //count = 0;
    }
}
