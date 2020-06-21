using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionAndPositionLerping : MonoBehaviour
{
    public bool LerpOnStart = false;
   

    [Header("Height Settings"), Space(5)]
    public float HeightLerpStart;
    public float HeightLerpEnd;
    
    [Header("Panel Updates"), Space(10)]
    public Color DefaultPanelColor = Color.black;
    public Color PressingPanelColor = Color.white;
    public Color ActivatedPanelColor = Color.red;

    [Header("Ground Updates"), Space(10)]
    public Color GroundStartColor = Color.white;
    public Color GroundEndColor = Color.red;
    [Space(5)]
    public Color GroundStartColorAdd = Color.white;
    public Color GroundEndColorAdd = Color.red;
    [Space(5)]
    public Color GroundStartColorMultiply = Color.white;
    public Color GroundEndColorMultiply = Color.red;

    [Header("References"), Space(10)]
    public Transform TargetTransform;
    public Renderer TargetRenderer;
    public Renderer GroundRenderer;

    public float InitialLerpRange = 0.2f;
    public bool Lerping = false;

    private float previousPos;

    private Material MaterialInstance;

    #region Unity API

    void Start()
    {
        if(LerpOnStart)
        {
            StartLerp();
        }
    }

    void Update()
    {
        if(Lerping)
        {
            Lerp();
        }
    }

    #endregion

    #region Lerp

    [Button("Start Lerping", ButtonSizes.Large)]
    public void StartLerp()
    {
        if(MaterialInstance == null)
        {
            CreateMaterialInstance();
        }
        Lerping = true;
    }

    [Button("End Lerping", ButtonSizes.Large)]
    public void EndLerp()
    {
        Lerping = false;
        SetGroundToDefault();
        DestroyMaterialInstance();
    }

    public void Lerp()
    {
        float lerpStep = Helpers.ConvertLinearRange(TargetTransform.localPosition.y, HeightLerpStart, HeightLerpEnd, 0, 1);
        float updatedLerpStep = 0f;

        if (previousPos == TargetTransform.localPosition.y)
        {
            return;
        }

        if(MaterialInstance)
        {
            if(lerpStep < InitialLerpRange)
            {
                SetGroundToDefault();
                updatedLerpStep = Helpers.ConvertLinearRange(lerpStep, 0f, InitialLerpRange, 0.5f, 1f);
                MaterialInstance.SetColor("_EmissionColor", Color.Lerp(DefaultPanelColor, DefaultPanelColor, updatedLerpStep));
            }
            else
            {
                SetGroundToWarning();
                updatedLerpStep = Helpers.ConvertLinearRange(lerpStep, InitialLerpRange, 1f, 0, 1);
                MaterialInstance.SetColor("_EmissionColor", Color.Lerp(PressingPanelColor, ActivatedPanelColor, updatedLerpStep));
            }
           
        }
    }

    #endregion

    #region Materials

    public void CreateMaterialInstance()
    {
        MaterialInstance = TargetRenderer.material;
    }

    public void DestroyMaterialInstance()
    {
        Destroy(MaterialInstance);
    }

    #endregion


    public void SetGroundToWarning()
    {
        GroundRenderer.sharedMaterial.SetColor("_EmissionColor", GroundEndColor);
        GroundRenderer.sharedMaterial.SetColor("_ColorMultiply", GroundEndColor);
        GroundRenderer.sharedMaterial.SetColor("_ColorAdd", GroundEndColor);
    }

    public void SetGroundToDefault()
    {
        GroundRenderer.sharedMaterial.SetColor("_EmissionColor", GroundStartColor);
        GroundRenderer.sharedMaterial.SetColor("_ColorMultiply", GroundStartColor);
        GroundRenderer.sharedMaterial.SetColor("_ColorAdd", GroundStartColor);
    }

}
