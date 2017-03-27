﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour {

    #region Properties
    [SerializeField]
    private Image originalImage, extraImage;
    public Color[] OriginalImageSpritePixels
    {
        get
        {
            return originalImagePixels;
        }
    }
    private Color[] originalImagePixels;
    [SerializeField]
    private Dropdown imageProcessingMethodsDropdown;

    #region Singleton
    private static UIManager instance = null;
    public static UIManager Instance
    {
        get
        {
            return instance;
        }
    }
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
        originalImagePixels = originalImage.sprite.texture.GetPixels();
        Texture2D temp = new Texture2D(originalImage.sprite.texture.width, originalImage.sprite.texture.height);
        temp.SetPixels(originalImagePixels);
        temp.Apply();

        extraImage.sprite = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), new Vector2(0.5f, 0.5f));

        extraImage.preserveAspect = true;
        imageProcessingMethodsDropdown.onValueChanged.AddListener(ChooseEffect);
    }
    #endregion

    public void FillImageProcessingMethods()
    {
        imageProcessingMethodsDropdown.ClearOptions();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        foreach (var item in ImageProcessingManager.Instance.AvaibleProcessingMethods)
        {
            options.Add(new Dropdown.OptionData { text = item.Key });
        }
        imageProcessingMethodsDropdown.AddOptions(options);
    }
    private void ChooseEffect(int numb)
    {
        ImageProcessingManager.Instance.AvaibleProcessingMethods[imageProcessingMethodsDropdown.options[numb].text].DynamicInvoke(extraImage.sprite);
    }
    #endregion
}