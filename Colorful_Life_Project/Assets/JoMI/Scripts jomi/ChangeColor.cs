using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jomi.interactableSystem;

public class ChangeColor : Interactable
{
    private MeshRenderer _meshRenderer;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }



    protected override void Interact()
    {
        _meshRenderer.material.SetColor("_Color", new Vector4(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f),1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
