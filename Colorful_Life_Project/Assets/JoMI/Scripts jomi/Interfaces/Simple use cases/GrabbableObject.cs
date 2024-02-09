using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour, IGrabbable
{
    private Rigidbody _rb;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();    
    }


    #region interface
    [SerializeField] private float _offSet;
    [SerializeField] private Vector3 _handRotation;

    public float Offset => _offSet;

    public (Quaternion, Quaternion) HandsRotations => (Quaternion.Euler( _handRotation), Quaternion.Euler( new Vector3(_handRotation.x, _handRotation.y, -_handRotation.z)));

    public Vector3 Position => transform.position;

    public GameObject GrabbableGO => this.gameObject;

    public void Grab() => _rb.useGravity = false;
    


    public void UnGrab() => _rb.useGravity = true;

    public void updatePosWithHandsPos(Vector3 middleHandsPos) => transform.position = middleHandsPos;
    
    #endregion
}
