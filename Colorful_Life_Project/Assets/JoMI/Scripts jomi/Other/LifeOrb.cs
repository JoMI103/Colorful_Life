using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeOrb : MonoBehaviour
{

    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] DestroyOnTime _destroyParticleSystem;
    private void OnTriggerStay(Collider other)
    {

        if (other.TryGetComponent(out PlayerContext _ctx)){
            if(_ctx.PlayerInfo.CurrentHP < _ctx.PlayerInfo.MaxHP)
            {
                _ctx.PlayerInfo.CurrentHP += 10;
                _particleSystem.Play();
                _destroyParticleSystem.gameObject.SetActive(true);
                Destroy(this.gameObject);
            }
        } 
      
    }

}
