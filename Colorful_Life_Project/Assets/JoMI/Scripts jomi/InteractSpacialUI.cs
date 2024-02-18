using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSpacialUI : MonoBehaviour
{
    bool _show;

    [SerializeField] private GameObject interactText;
   
    public Vector3 offset;

    private Transform _target;
    [SerializeField] Camera _mainCamera;

    public Transform Target { get => _target; set => _target = value; }
    public bool Show { get => _show; set { if (_show != value) interactText.SetActive(value); _show = value; } }





    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = _mainCamera.WorldToScreenPoint(_target.position + offset);
    }

}
