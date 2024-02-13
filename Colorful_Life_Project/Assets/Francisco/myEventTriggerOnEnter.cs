using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class myEventTriggerOnEnter : MonoBehaviour
{

    [Header("Custom Event")]
    public UnityEvent myEvents;


    private void OnTriggerEnter(Collider other)
    {
        if(myEvents != null)
        {
            print("deu mas esta null");
        }

        else
        {
            print("deu e tem o evento" + myEvents);
            myEvents.Invoke();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
