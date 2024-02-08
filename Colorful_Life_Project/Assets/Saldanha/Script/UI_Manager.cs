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

    private void UpdateDiaryUI(DiaryPage page)
    {
        Debug.Log("Tentando ativar o painel do di�rio");
        diaryPanel.SetActive(true);


        //Maneira generica de instanciar as p�ginas
        /*foreach (DiaryPage pageUnlocked in _diaryManager._unlockedPages)
        {
            Debug.Log("Tentando instanciar a p�gina");

            // Verificar se o �ndice est� dentro do intervalo da lista
            if (pageUnlocked._id >= 0 && pageUnlocked._id < diaryPagePrefab.Count)
            {
                GameObject pagePrefab = Instantiate(diaryPagePrefab[pageUnlocked._id], diaryPanel.transform);
                pagePrefab.transform.Find("TitleText").GetComponent<TextMeshProUGUI>().text = pageUnlocked._title;
                pagePrefab.transform.Find("TypeText").GetComponent<TextMeshProUGUI>().text = pageUnlocked._type;
                pagePrefab.transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>().text =pageUnlocked._content;
            }
            
        }*/
    }

    public void NextPage()
    {
        if (currentPageIndex < diaryPagePrefab.Count - 1 && currentPageIndex < _diaryManager._unlockedPages.Count - 1)
        {
            diaryPagePrefab[currentPageIndex].SetActive(false);
            currentPageIndex++;
            diaryPagePrefab[currentPageIndex].SetActive(true);
        }
    }

    public void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            diaryPagePrefab[currentPageIndex].SetActive(false);
            currentPageIndex--;
            diaryPagePrefab[currentPageIndex].SetActive(true);
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

    public void SetSlideMaxDespair(int maxDespair)
    {
        _despairSlider = GameObject.Find("DespairSlider").GetComponent<Slider>();

        _despairSlider.maxValue = maxDespair;
        _despairSlider.value = maxDespair;
    }

    public void SetSlideDespair(float currentDispair)
    {
        _despairSlider.value = currentDispair / _despairSlider.maxValue * 100f;
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
            case PlayerInfo.Magic.Sadness:
                _abilitySlots[0].SetActive(true);
                break;
            case PlayerInfo.Magic.Rage:
                _abilitySlots[1].SetActive(true);
                break;
            case PlayerInfo.Magic.Guilt:
                _abilitySlots[2].SetActive(true);
                break;
        }
    }

    #endregion

    #endregion

}
