using System.Collections;
using System.IO;
using UnityEngine;

public class ImageCapturing : MonoBehaviour
{
    private Texture2D cachedTexture;
    private RenderTexture renderTexture;
    private Texture2D defaultTexture;
    private Vector2 screenRatio = new Vector2(1f, 1f);
    [SerializeField] Camera captureCamera;

    private void Awake()
    {
        renderTexture = new RenderTexture(Screen.width, Screen.height, 300);
        InitDefaultTexture();
    }

    private void InitDefaultTexture()
    {
        defaultTexture = new Texture2D(1, 1);
        Color[] pixels = defaultTexture.GetPixels();
        int i = 0;
        for (int num = pixels.Length; i < num; i++)
        {
            pixels[i] = Color.white;
        }
        defaultTexture.SetPixels(pixels);
        defaultTexture.Apply();
    }

    public void TakeLevelScreenshot(Rect rect)
    {
        StartCoroutine(IEOnImageCaptureRequest(rect));
    }

    IEnumerator IEOnImageCaptureRequest(Rect boundary)
    {
        yield return new WaitForEndOfFrame();
        if (!(boundary.width < 64f) && !(boundary.height < 64f))
        {
            captureCamera.targetTexture = renderTexture;
            RenderTexture.active = renderTexture;
            captureCamera.Render();
            if (cachedTexture != null)
            {
                Destroy(cachedTexture);
            }
            cachedTexture = new Texture2D(Mathf.FloorToInt(boundary.width), Mathf.FloorToInt(boundary.height), TextureFormat.RGB24, mipChain: false);
            cachedTexture.ReadPixels(boundary, 0, 0);
            cachedTexture.Apply();
            string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
            File.WriteAllBytes(filePath, cachedTexture.EncodeToPNG());
            Destroy(cachedTexture);
            new NativeShare().AddFile(filePath).SetSubject("Get for free now!").SetText("Da vinci would be proud of you!").SetTitle("No War").Share();
            RenderTexture.active = null;
            captureCamera.targetTexture = null;
        }
    }
    public void ScreenshotAndShare()
    {
        StartCoroutine(TakeScreenshotAndShare());
    }

    private IEnumerator TakeScreenshotAndShare()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());

        // To avoid memory leaks
        Destroy(ss);
        //Debug.LogError(filePath);
        new NativeShare().AddFile(filePath).SetSubject("Get for free now!").SetText("Da vinci would be proud of you!").SetTitle("No War").Share();

        // Share on WhatsApp only, if installed (Android only)
        //if( NativeShare.TargetExists( "com.whatsapp" ) )
        //	new NativeShare().AddFile( filePath ).AddTarget( "com.whatsapp" ).Share();
    }
}
