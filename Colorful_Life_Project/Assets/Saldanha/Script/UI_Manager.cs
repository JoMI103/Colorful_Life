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
    private Slider _lifeSlider;
    private Slider _despairSlider;
    #endregion
    #region [Singleton]
    public static UI_Manager Instance { get; private set; }

    public void Awake()
    {


        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // This object will not be destroyed when loading a new scene
        }
        else
        {
            Destroy(gameObject);
        }
    }
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
        DiaryManager.Instance.OnPageUnlocked += UpdateDiaryUI;
    }


    private void OnDisable()
    {
        DiaryManager.Instance.OnPageUnlocked -= UpdateDiaryUI;
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
        _lifeSlider = GameObject.Find("LifeSlider").GetComponent<Slider>();

        _lifeSlider.maxValue = maxLifeHP;
        _lifeSlider.value = maxLifeHP;
    }



    public void SetSlideLife(int lifeHP)
    {
        lifeHP = (int)Mathf.Clamp(lifeHP, 0, _lifeSlider.maxValue);

        float targetValue = lifeHP / _lifeSlider.maxValue * 100;

        StartCoroutine(UpdateSliderGradually(targetValue));
    }

    private IEnumerator UpdateSliderGradually(float targetValue)
    {
        float timeToChange = 1f;
        float startValue = _lifeSlider.value;
        float elapsedTime = 0f;

        while (elapsedTime < timeToChange)
        {
            elapsedTime += Time.deltaTime;
            _lifeSlider.value = Mathf.Lerp(startValue, targetValue, elapsedTime / timeToChange);
            yield return null;
        }
        _lifeSlider.value = targetValue;
    }

    #endregion

    #region DespairSlider

    public void SetSlideMaxDespair(float maxDespair)
    {
        _despairSlider = GameObject.Find("DespairSlider").GetComponent<Slider>();

        _despairSlider.maxValue = maxDespair;
    }

    public void SetSlideDespair(float currentDispair)
    {
        _despairSlider.value = currentDispair ;
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
