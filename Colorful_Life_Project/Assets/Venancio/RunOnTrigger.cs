using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RunOnTrigger : MonoBehaviour
{
    [SerializeField]
    private Collider _collider;
    public bool IsDetectingCollisions = true;

    public OnTriggerEvent OnTriggerEnterEvent;
    public OnTriggerEvent OnTriggerStayEvent;
    public OnTriggerEvent OnTriggerExitEvent;

    private void Awake()
    {
        _collider = _collider ? _collider : gameObject.GetComponent<Collider>();
        if (!_collider.isTrigger) _collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsDetectingCollisions)
        {
            OnTriggerEnterEvent.Invoke(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsDetectingCollisions)
        {
            OnTriggerStayEvent.Invoke(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsDetectingCollisions)
        {
            OnTriggerExitEvent.Invoke(other);
        }
    }

}

[Serializable]
public class OnTriggerEvent : UnityEvent<Collider> { };
