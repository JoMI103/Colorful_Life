using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatform :MonoBehaviour, IHittable
{
    [SerializeField] int nHits;

    public void Hit(GameObject hittedBy, Vector3 hitDirection, Vector3 inpactPosition, int damage)
    {
        if (hittedBy.GetComponent<PlayerContext>() == null) return;
        nHits--;
        if (nHits < 0) Killed();
    }

    public void Killed()
    {
        Destroy(this.gameObject);
    }

    
}
