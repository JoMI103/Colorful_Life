using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Singleton<T> : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected void InitializeSingleton()
    {
        if (Instance != null && Instance.Equals(this))
        {
            Destroy(this);
        }
        else
        {
            Instance = (T) this.ConvertTo(typeof(T));
        }
    }

    private void Awake()
    {
        InitializeSingleton();
    }
}
