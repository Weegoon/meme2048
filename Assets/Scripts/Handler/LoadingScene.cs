using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField]
    Image fillImage;
    [SerializeField]
    GameObject Container;
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        ReSize();
        fillImage.fillAmount = 0;
        StartCoroutine(LoadinngScene(1));
        //SceneManager.LoadSceneAsync(1);
    }
    private void ReSize()
    {
        var aspect = 1f / Camera.main.aspect;
        var scale = 2f / aspect;
        if (aspect > 2f)
        {
            Container.transform.localScale = new Vector3(scale, scale, 1);
        }
    }
    float count = 0;
    private void Update()
    {
        //count ++;
        //if(count<2000)
        //{
        //    float progessValue = count / 2000;
        //    fillImage.fillAmount = progessValue;
        //}   
        //else
        //{
        //    fillImage.fillAmount = 1;
        //}    
    }

    // Update is called once per frame
    IEnumerator LoadinngScene(int sceneId)
    {
        //AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        while (count / 2f < 1)//!operation.isDone
        {
            float progessValue = count / 2f;
            yield return null;
            count += 0.01f;
            fillImage.fillAmount = progessValue;
        }

        Bridge.instance.OnGameReady();

        if (LocalStore.IsFirst())
            SceneManager.LoadSceneAsync(2);
        else
            SceneManager.LoadSceneAsync(1);
    }
}
