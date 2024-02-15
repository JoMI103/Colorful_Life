using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class myEventTriggerOnEnter : MonoBehaviour
{

    public Animator animEnimL;
    public Animator animEnimR;
    public Animator animBoxL;
    public Animator animBoxR;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerContext>() == null) return;
        
        animEnimL.Play("Edge_L");
        animEnimR.Play("Edge_R");
        animBoxL.Play("Crate_Drop_L");
        animBoxR.Play("Crate_Drop_R");

        Destroy(this.gameObject);
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
