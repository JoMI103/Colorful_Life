using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    PlayerContext _playerContext;

    public void Explosion()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, 10);

        foreach (Collider col in colliders)
        {
            MonoBehaviour[] allScripts = col.gameObject.GetComponentsInChildren<MonoBehaviour>();
            foreach (MonoBehaviour mono in allScripts)
            {
                if (mono is IHittable)
                {
                    (mono as IHittable).Hit(this.gameObject, (mono.transform.position - this.transform.position).normalized * 100, mono.transform.position, 10);
                }
            }
        }
    }
}
