using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ImpactableObject : MonoBehaviour, IHittable
{
    private Rigidbody _rb;

    private void Start()
    { 
        _rb = GetComponent<Rigidbody>();    
    }

    public void Hit(GameObject hittedBy, Vector3 hitDirection, Vector3 inpactPosition, int damage)
    {
        _rb.AddForceAtPosition(hitDirection, inpactPosition, ForceMode.Impulse);
    }

    public void Killed()
    {
       
    }

}
