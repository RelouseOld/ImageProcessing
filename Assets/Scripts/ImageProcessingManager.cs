﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageProcessingManager : MonoBehaviour {

    #region Variables
    private Dictionary<string, System.Delegate> avaibleProcessingMethods = new Dictionary<string, System.Delegate>();
    public Dictionary<string, System.Delegate> AvaibleProcessingMethods
    {
        get
        {
            return avaibleProcessingMethods;
        }
    }
    private delegate void ApplyEffect(Sprite sprite);
    
    #region Singleton
    private static ImageProcessingManager instance = null;
    public static ImageProcessingManager Instance
    {
        get
        {
            return instance;
        }
    }
    private float _binarizationBorder;
    
    #endregion
    #endregion
    #region Methods
    #region UnityMethods
    private void Awake()
    {
        #region Singleton implementation
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
            return;
        }
        #endregion
    }
    private void Start()
    {
        avaibleProcessingMethods.Add("Original", new ApplyEffect(ApplyOriginal));
        avaibleProcessingMethods.Add("Negative", new ApplyEffect(ApplyNegative));
        avaibleProcessingMethods.Add("Binarization", new ApplyEffect(StartBinarization));
        avaibleProcessingMethods.Add("Shades of gray", new ApplyEffect(ApplyShadesOfGray));
        //avaibleProcessingMethods.Add("Brightness", new ApplyEffect(ApplyShadesOfGray));
        if (avaibleProcessingMethods != null)
            UIManager.Instance.FillImageProcessingMethods();

    }
    private void ApplyOriginal(Sprite sprite)
    {
        sprite.texture.SetPixels(UIManager.Instance.OriginalImageSpritePixels);
        sprite.texture.Apply();
    }
    
    private void ApplyNegative(Sprite sprite)
    {
        Color[] pixels = sprite.texture.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = new Color(1 - pixels[i].r, 1 - pixels[i].g, 1 - pixels[i].b);
        }
        sprite.texture.SetPixels(pixels);
        sprite.texture.Apply();
    }
    public void ChangeBinarizationBorderValue(float value)
    {
        _binarizationBorder = value;
    }
    private void StartBinarization(Sprite sprite)
    {
        UIManager.Instance.BinarizationWindowTools.SetActive(true);
    }
    
    public void ApplyBinarization()
    {
        Sprite sprite = UIManager.Instance.ExtraImage.sprite;
        Color[] pixels = sprite.texture.GetPixels();
        ApplyOriginal(sprite);
        Debug.Log(_binarizationBorder);
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = GetColorBrightness(pixels[i]) > _binarizationBorder ? Color.white : Color.black;
        }
        sprite.texture.SetPixels(pixels);
        sprite.texture.Apply();
    }
    private float GetColorBrightness(Color color)
    {
        float temp = Mathf.Sqrt(color.r * color.r * 0.241f + color.g * color.g * 0.691f + color.b * color.b * 0.068f);
        return temp;
    }
    private void ApplyShadesOfGray(Sprite sprite)
    {
        Color[] pixels = sprite.texture.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            float tempFl = pixels[i].r * 0.3f + pixels[i].g * 0.59f + pixels[i].b * 0.11f;

            pixels[i] = new Color(tempFl, tempFl, tempFl);
        }
        sprite.texture.SetPixels(pixels);
        sprite.texture.Apply();
    }
    public void ApplyBrightness(Sprite sprite)
    {

    }
    #endregion
    #endregion
}
