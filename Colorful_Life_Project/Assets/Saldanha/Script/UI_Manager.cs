using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    #region HUD REFERENCES
    [Header("HUD")]
    [SerializeField] private Image _lifeImage;
    [SerializeField] private Image _despairImage;
    #endregion



    #region[Diario]
    [Header("DiarioManager")]
    [SerializeField] DiaryManager _diaryManager;
    [SerializeField] GameObject diaryPanel;
    [SerializeField] List<GameObject> diaryPagePrefab;

    //UI elements, move page
    private int currentPageIndex = 0;

    private void OnEnable()
    {
        _diaryManager.OnPageUnlocked += UpdateDiaryUI;
    }


    private void OnDisable()
    {
        _diaryManager.OnPageUnlocked -= UpdateDiaryUI;
    }

    public void EnableDiary()
    {
          diaryPanel.SetActive(true);
    }

    public void UpdateDiaryUI(DiaryPage page)
    {
        
        diaryPanel.SetActive(true);

        for (int i = 0; i < _diaryManager._unlockedPages.Count; i += 2)
        {
            
            if (i < diaryPagePrefab.Count)
            {
                var currentPage = _diaryManager._unlockedPages[i];
                var currentPagePrefab = diaryPagePrefab[i];
                UpdatePageUI(currentPagePrefab, currentPage);
               
            }
            if (i + 1 < _diaryManager._unlockedPages.Count && i + 1 < diaryPagePrefab.Count)
            {
                var nextPage = _diaryManager._unlockedPages[i + 1];
                var nextPagePrefab = diaryPagePrefab[i + 1];
                UpdatePageUI(nextPagePrefab, nextPage);
                
            }
        }
    }
    
    private void UpdatePageUI(GameObject pagePrefab, DiaryPage page)
    {
        pagePrefab.transform.Find("TitleText").GetComponent<TextMeshProUGUI>().text = page._title;
        pagePrefab.transform.Find("TypeText").GetComponent<TextMeshProUGUI>().text = page._type;
        pagePrefab.transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>().text = page._content;
        pagePrefab.transform.Find("Image").GetComponent<Image>().gameObject.SetActive(false);
    }


    public void NextPage()
    {
        if (currentPageIndex < diaryPagePrefab.Count - 2 && currentPageIndex < _diaryManager._unlockedPages.Count - 2)
        {
            diaryPagePrefab[currentPageIndex].SetActive(false);
            diaryPagePrefab[currentPageIndex+1].SetActive(false);
            currentPageIndex +=2;
            diaryPagePrefab[currentPageIndex].SetActive(true);
            diaryPagePrefab[currentPageIndex+1].SetActive(true);
        }
    }

    public void PreviousPage()
    {
        if (currentPageIndex > 1)
        {
            diaryPagePrefab[currentPageIndex].SetActive(false);
            diaryPagePrefab[currentPageIndex+1].SetActive(false);
            currentPageIndex -=2;
            diaryPagePrefab[currentPageIndex].SetActive(true);
            diaryPagePrefab[currentPageIndex+1].SetActive(true);
        }
    }

    #endregion

    #region HUD 
    #region LifeSlider
    public void SetSlideMaxLife(int maxLifeHP)
    {

        _lifeImage.fillAmount = maxLifeHP/100f;
       
    }



    public void SetSlideLife(int lifeHP)
    {
        lifeHP = (int)Mathf.Clamp(lifeHP, 0, 100);

        float targetValue = lifeHP / 100f;

        StartCoroutine(UpdateSliderGradually(_lifeImage, targetValue));
    }

    private IEnumerator UpdateSliderGradually(Image image,float targetValue)
    {
        float timeToChange = 1f;
        float startValue = image.fillAmount;
        float elapsedTime = 0f;

        while (elapsedTime < timeToChange)
        {
            elapsedTime += Time.deltaTime;
            image.fillAmount = Mathf.Lerp(startValue, targetValue, elapsedTime / timeToChange);
            yield return null;
        }
        image.fillAmount = targetValue;
    }

    #endregion

    #region DespairSlider

    public void SetSlideMaxDespair(float maxDespair)
    {
        //_despairSlider = GameObject.Find("DespairSlider").GetComponent<Slider>();

        _despairImage.fillAmount = 0;// maxDespair / 100;
    }

    public void SetSlideDespair(float currentDispair)
    {
        _despairImage.fillAmount = currentDispair ;
    }

    #endregion

    #region AbilitiesSlots

    [Header("AbilitiesSlots")]
    [SerializeField] private List<GameObject> _abilitySlots;

    public void UnlockAbilities(PlayerInfo.Magic currentMagic)
    {
        _abilitySlots[0].SetActive(false);
        _abilitySlots[1].SetActive(false);
        _abilitySlots[2].SetActive(false);
        switch (currentMagic)
        {
            case PlayerInfo.Magic.Rage:
                _abilitySlots[0].SetActive(true);
                break;
            case PlayerInfo.Magic.Guilt:
                _abilitySlots[1].SetActive(true);
                break;
            case PlayerInfo.Magic.Sadness:
                _abilitySlots[2].SetActive(true);
                break;
        }
    }

    #endregion

    #endregion

}
