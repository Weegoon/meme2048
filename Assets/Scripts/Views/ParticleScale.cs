using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScale : MonoBehaviour
{
    ParticleSystem ps;
    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }
    private void OnEnable()
    {
        float aspect = 1f / Camera.main.aspect;
        float scale = 1.6889f / aspect;
        if (aspect > 1.6889f)
        {
            this.transform.localScale = new Vector3(scale, scale, 1);
            ps.transform.localScale = new Vector3(scale, scale, scale);
            var main = ps.main;
            main.scalingMode = ParticleSystemScalingMode.Local;
            foreach (Transform child in this.transform)
            {
                child.localScale = new Vector3(scale, scale, scale);
                ParticleSystem.MainModule settings = child.GetComponent<ParticleSystem>().main;
                settings.scalingMode = ParticleSystemScalingMode.Local;
            }
        }
        else
        {
            scale = 0.95f;
            this.transform.localScale = new Vector3(scale, scale, 1);
            ps.transform.localScale = new Vector3(scale, scale, scale);
            var main = ps.main;
            main.scalingMode = ParticleSystemScalingMode.Local;
            foreach (Transform child in this.transform)
            {
                child.localScale = new Vector3(scale, scale, scale);
                ParticleSystem.MainModule settings = child.GetComponent<ParticleSystem>().main;
                settings.scalingMode = ParticleSystemScalingMode.Local;
            }
        }   

    }
    void Update()
    {
        //float aspect = 1f / Camera.main.aspect;
        //float scale = 2f / aspect;
        //if (aspect > 2f)
        //{
        //    this.transform.localScale = new Vector3(scale, scale, 1);
        //    ps.transform.localScale = new Vector3(scale, scale, scale);
        //    var main = ps.main;
        //    main.scalingMode = ParticleSystemScalingMode.Hierarchy;
        //    foreach (Transform child in this.transform)
        //    {
        //        child.localScale = new Vector3(scale, scale, scale);
        //        ParticleSystem.MainModule settings = child.GetComponent<ParticleSystem>().main;
        //        settings.scalingMode = ParticleSystemScalingMode.Hierarchy;
        //    }
        //}
        //ps.transform.localScale = new Vector3(sliderValue, sliderValue, sliderValue);
        //if (ps.transform.parent != null)
        //    ps.transform.parent.localScale = new Vector3(parentSliderValue, parentSliderValue, parentSliderValue);

        //var main = ps.main;
        //main.scalingMode = scaleMode;
    }

    void OnGUI()
    {
        //scaleMode = (ParticleSystemScalingMode)GUI.SelectionGrid(new Rect(25, 25, 300, 30), (int)scaleMode, new GUIContent[] { new GUIContent("Hierarchy"), new GUIContent("Local"), new GUIContent("Shape") }, 3);
        //GUI.Label(new Rect(25, 80, 100, 30), "Scale");
        //sliderValue = GUI.HorizontalSlider(new Rect(125, 85, 100, 30), sliderValue, 0.0F, 5.0F);
        //GUI.Label(new Rect(25, 120, 100, 30), "Parent Scale");
        //parentSliderValue = GUI.HorizontalSlider(new Rect(125, 125, 100, 30), parentSliderValue, 0.0F, 5.0F);
    }
}
