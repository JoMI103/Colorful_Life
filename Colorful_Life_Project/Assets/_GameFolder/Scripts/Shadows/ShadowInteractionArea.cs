using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowInteractionArea : MonoBehaviour
{
    public ShadowManager ShadowManager;
    public GameObject ShadowObject;

    
    private void OnTriggerEnter(Collider other)
    {
        ShadowManager.EnableMe(ShadowObject);
    }

    //private void OnTriggerStay(Collider other)
    //{
    //}

    private void OnTriggerExit(Collider other)
    {
        ShadowManager.DisableMe(ShadowObject);
    }
}
