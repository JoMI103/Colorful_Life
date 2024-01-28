using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DiarioInter : MonoBehaviour, IInteractable
{

    [SerializeField] private string _promptMessage;

    [SerializeField] private UI_Manager _uiManager;
    public string PromptMessage { get; set; }

    public GameObject PlayerGO { get; set; }

    public void Interact(GameObject player)
    {
        PlayerGO = player;
        _uiManager
        _meshRenderer.material.SetColor("_BaseColor", new Vector4(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1));
    }

    private MeshRenderer _meshRenderer;

    private void Start()
    {
        PromptMessage = _promptMessage;
        _meshRenderer = GetComponent<MeshRenderer>();
    }


   
}
