using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor2 : MonoBehaviour, IInteractable
{

    [SerializeField] private string _promptMessage;
    public string PromptMessage { get; set; }

    public GameObject PlayerGO { get; set; }

    public void Interact(GameObject player)
    {
        PlayerGO = player;
        _meshRenderer.material.SetColor("_Color", new Vector4(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1));
    }

    private MeshRenderer _meshRenderer;

    private void Start()
    {
        PromptMessage = _promptMessage;
        _meshRenderer = GetComponent<MeshRenderer>();
    }


   
}
