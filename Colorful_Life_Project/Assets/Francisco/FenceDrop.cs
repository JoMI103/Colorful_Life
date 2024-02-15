using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceDrop : MonoBehaviour
{

    public Animator animL;
    public Animator animR;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerContext>() == null) return;

        animL.Play("FenceDrop_L");
        animR.Play("FenceDrop_R");
       
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
