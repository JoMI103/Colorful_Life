using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Masks : MonoBehaviour, IGrabbable, IMask
{
    [SerializeField] private int _maskGroup;
    [SerializeField] private string _maskText;
    [SerializeField] private bool _maskPlaced;
    [SerializeField] private Vector3 _targetPos;

    public int GroupMask => _maskGroup;
    public Masks MaskScript => this;
    public bool MaskPlaced { get => _maskPlaced; set => _maskPlaced = value; }
    public Vector3 TargetPos { get => _targetPos; set { _targetPos = value; transform.position = _targetPos;  } }



    public float Offset => 1;

    public (Quaternion, Quaternion) HandsRotations => (Quaternion.identity, Quaternion.identity);

    public Vector3 Position => transform.position;

    public GameObject GrabbableGO => this.gameObject;


    public void Grab()
    {
        Debug.Log("Aparece texto da mascara");
    }

    public void UnGrab()
    {
        Debug.Log("Desaparece texto da mascara");
    }

    public void updatePosWithHandsPos(Vector3 middleHandsPos)
    {
        transform.position = middleHandsPos;
    }
}
