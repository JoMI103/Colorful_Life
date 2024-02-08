using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    #region HUD REFERENCES
    [Header("HUD")]
    private Slider _lifeSlider;
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
        Debug.Log("Tentando ativar o painel do diário");
        diaryPanel.SetActive(true);


        //Maneira generica de instanciar as páginas
        /*foreach (DiaryPage pageUnlocked in _diaryManager._unlockedPages)
        {
            Debug.Log("Tentando instanciar a página");

            // Verificar se o índice está dentro do intervalo da lista
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
        if (currentPageIndex < diaryPagePrefab.Count - 1 && currentPageIndex <_diaryManager._unlockedPages.Count-1)
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

    public void SetSlideMaxLife(int maxLifeHP)
    {
        _lifeSlider = GameObject.Find("LifeSlider").GetComponent<Slider>();

        _lifeSlider.maxValue = maxLifeHP;
        _lifeSlider.value = maxLifeHP;
    }   



    public void SetSlideLife(int lifeHP)
    {
        lifeHP =(int)Mathf.Clamp(lifeHP, 0, _lifeSlider.maxValue);

        float targetValue = lifeHP/ _lifeSlider.maxValue;

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
                _lifeSlider.value = Mathf.Lerp(startValue, targetValue , elapsedTime / timeToChange);
                yield return null;
          }
       _lifeSlider.value = targetValue;
    }


    #endregion

}
