using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Masks : MonoBehaviour, IGrabbable, IMask
{
    public int maskGroup;
    public string maskText;



    public float Offset => 1;

    public (Quaternion, Quaternion) HandsRotations => throw new System.NotImplementedException();

    public Vector3 Position => transform.position;

    public GameObject GrabbableGO => this.gameObject;

    public void Grab()
    {
        Debug.Log("Aparece texto da mascara");
        throw new System.NotImplementedException();
    }

    public void UnGrab()
    {
        Debug.Log("Desaparece texto da mascara");
        throw new System.NotImplementedException();
    }

    public void updatePosWithHandsPos(Vector3 middleHandsPos)
    {
        transform.position = middleHandsPos;
    }
}
