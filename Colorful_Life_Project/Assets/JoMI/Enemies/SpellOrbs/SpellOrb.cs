using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PlayerInfo;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SpellOrb : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayerMask;
    [SerializeField] private Magic _orbMagic;


    private void OnTriggerStay(Collider other)
    {
        if (_playerLayerMask == (_playerLayerMask | (1 << other.gameObject.layer)))
        {
            if (other.GetComponent<PlayerContext>().AddSpell(_orbMagic)) Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        
    }
}
