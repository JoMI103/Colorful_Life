using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryManager : MonoBehaviour
{
    public static DiaryManager Instance { get; private set; }

    public List<DiaryPage> _unlockedPages = new List<DiaryPage>();

    public event Action<DiaryPage> OnPageUnlocked;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // This object will not be destroyed when loading a new scene
            _unlockedPages = new List<DiaryPage>();
        }
        else
        {
            Destroy(gameObject);
        }
     
    }

    public void UnlockPage(DiaryPage page)
    {
        if (!_unlockedPages.Contains(page))
        {
            _unlockedPages.Add(page);
            OnPageUnlocked?.Invoke(page);
        }
    }

}
