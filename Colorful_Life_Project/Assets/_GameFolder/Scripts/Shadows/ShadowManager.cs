using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowManager : MonoBehaviour
{
    public List<ShadowArea> ShadowAreas;
    public GameObject CurrentShadow;


    private void Awake()
    {
        DisableAllShadows();
        CurrentShadow?.SetActive(true);
        SetupInteractionAreas();
    }

    private void SetupInteractionAreas()
    {
        foreach (ShadowArea shadowArea in ShadowAreas)
        {
            ShadowInteractionArea interactionArea = shadowArea.Collider.gameObject.AddComponent<ShadowInteractionArea>();
            interactionArea.ShadowManager = this;
            interactionArea.ShadowObject = shadowArea.Shadow;
        }
    }

    private void DisableAllShadows()
    {
        foreach (ShadowArea shadowArea in ShadowAreas)
        {
            shadowArea.Shadow.SetActive(false);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void EnableMe(GameObject shadow)
    {
        CurrentShadow.SetActive(false);
        shadow.SetActive(true);
        CurrentShadow = shadow;
        AudioManager.Instance.Play("ShadowPoof",shadow,"SFX");
    }

    internal void DisableMe(GameObject shadow)
    {
        shadow.SetActive(false);
    }
}

[Serializable]
public struct ShadowArea
{
    [SerializeField]
    public GameObject Shadow;
    [SerializeField]
    public Collider Collider;
}
