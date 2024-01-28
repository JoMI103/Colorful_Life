using System;
using System.Collections;
using System.Collections.Generic;
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
       


    }

    #endregion



}
