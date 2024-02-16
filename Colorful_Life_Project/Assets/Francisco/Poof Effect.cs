using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoofEffect : MonoBehaviour
{

    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] DestroyOnTime _destroyParticleSystem;
    private void OnTriggerStay(Collider other)
    {

       
            
                
                _particleSystem.Play();
                _destroyParticleSystem.gameObject.SetActive(true);
                Destroy(this.gameObject);
            
        

    }

}
