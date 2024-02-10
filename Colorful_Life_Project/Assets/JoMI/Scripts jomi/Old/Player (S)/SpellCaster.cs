using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    [SerializeField] PlayerContext _playerContext;
    [SerializeField] Transform _explosionEffect;

    public void Explosion()
    {
        Instantiate(_explosionEffect);

        int dmg = _playerContext.PlayerBaseStats.ExplosionDmg;
        float impactForce = _playerContext.PlayerBaseStats.ExplosionForce;

        Collider[] colliders = Physics.OverlapSphere(transform.position, 10);

        foreach (Collider col in colliders)
        {
            MonoBehaviour[] allScripts = col.gameObject.GetComponentsInChildren<MonoBehaviour>();
            foreach (MonoBehaviour mono in allScripts)
            {
                if (mono is IHittable)
                {
                    (mono as IHittable).Hit(this.gameObject, (mono.transform.position - this.transform.position).normalized * impactForce, mono.transform.position, dmg);
                }
            }
        }
    }
}
