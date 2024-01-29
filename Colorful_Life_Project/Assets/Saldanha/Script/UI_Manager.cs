using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
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



}
