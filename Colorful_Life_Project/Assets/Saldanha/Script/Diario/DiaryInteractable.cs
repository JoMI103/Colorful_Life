using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DiaryInteractable : MonoBehaviour
{

   [SerializeField] DiaryPage _pageToUnlock;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           // Debug.LogWarning("Player entered");
            DiaryManager.Instance.UnlockPage(_pageToUnlock);
            //Debug.LogWarning("Page unlocked");
        }
    
    }
}


