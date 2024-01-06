using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class TNT : MonoBehaviour, IInteractable
{
    [SerializeField] private float radius;


    public string PromptMessage => "Explode";

    public GameObject PlayerGO { get; set; }

    public void Interact(GameObject player)
    {
        Explode();
    }


    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider col in colliders)
        {
            MonoBehaviour[] allScripts = col.gameObject.GetComponentsInChildren<MonoBehaviour>();
            foreach (MonoBehaviour mono in allScripts)
            {
                if(mono is IHittable)
                {
                    (mono as IHittable).Hit(this.gameObject,this.transform.position, 10);
                }
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, radius);
    }

}
