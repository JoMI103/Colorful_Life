using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas HUD;
    [SerializeField] private Image currentMagicImage;
    [SerializeField] private MagicDictionary[] magicImageDictionary;
    private UIManager.Magic _currentMagic;
    [SerializeField]
    public UIManager.Magic CurrentMagic
    {
        get
        {
            return _currentMagic;
        }
        set
        {
            MagicChanged = true;
            _currentMagic = value;
            UpdateCurrentMagicHUD();
        }
    }

    private bool MagicChanged = false;

    [Serializable]
    private struct MagicDictionary
    {
        public UIManager.Magic Magic;
        public Image Image;
    }

    void Update()
    {
     
    }

    private void UpdateCurrentMagicHUD()
    {
            foreach (MagicDictionary dictionaryPage in magicImageDictionary)
            {
                if (dictionaryPage.Magic.Equals(CurrentMagic))
                {
                    currentMagicImage.sprite = dictionaryPage.Image.sprite;
                    return;
                }
            }

            Debug.Log("UIManager: No image found. Check gameobject configuration.");
    }


    public void  EnableHUD (bool enable)
    {
        HUD.gameObject.SetActive(enable);
    }

    public enum Magic
    {
        Angry,
        Guilt,
        Sadness
    }

}