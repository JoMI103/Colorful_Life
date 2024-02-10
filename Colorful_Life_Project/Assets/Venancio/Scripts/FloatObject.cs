using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatObject : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    private Transform _transform;
    [SerializeField]
    private float _frequency;
    [SerializeField]
    private float _amplitude;
    [SerializeField]
    private Vector3 _initialPosition;
    [Header("Go to first Location Configuration")]
    [SerializeField]
    private bool _isGoingToStartLocation;
    [SerializeField]
    private Transform _startLocation;
    [SerializeField]
    private float _translationVelocyToStartLocation;
    [SerializeField]
    private float _distanteBetweenStartLocationAnTarget = 2e-05f;

    private void Awake()
    {
        if (_isGoingToStartLocation)
        {
            _initialPosition = _startLocation.position;
            
        } else
        {
            _initialPosition = _transform.position;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGoingToStartLocation &&
            Vector3.Distance(_transform.position, _startLocation.position) > _distanteBetweenStartLocationAnTarget)
        {
            _startLocation.position = new Vector3(_initialPosition.x, _initialPosition.y + MathF.Sin(Time.time * _frequency) * _amplitude, _initialPosition.z);
            _transform.position = Vector3.MoveTowards(_transform.position, _startLocation.position, _translationVelocyToStartLocation * Time.deltaTime);
        } else
        {
            _isGoingToStartLocation = false;
            _transform.position = new Vector3(_initialPosition.x, _initialPosition.y + MathF.Sin(Time.time * _frequency) * _amplitude, _initialPosition.z);
        }
    }
}

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class FloatObject : MonoBehaviour
//{
//    [Header("Setup")]
//    [SerializeField]
//    private Transform _transform;
//    [SerializeField]
//    private float _frequency;
//    [SerializeField]
//    private float _amplitude;
//    private Vector3 _initialPosition;
//    [Header("Go to first Location Configuration")]
//    [SerializeField]
//    private bool _isGoingToStartLocation;
//    [SerializeField]
//    private Transform _startLocation;
//    [SerializeField]
//    private float _translationVelocyToStartLocation;
//    private bool _isOnDestination = false;

//    private void Awake()
//    {
//        _initialPosition = _transform.position;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (_isGoingToStartLocation &&

//            !_isOnDestination)
//        {

//            _transform.position = Vector3.MoveTowards(_transform.position, _startLocation.position, _translationVelocyToStartLocation * Time.deltaTime);
//            if (Vector3.Distance(_transform.position, _startLocation.position) <= 0.5f) _isOnDestination = true;
//        }
//        else
//        {
//            _transform.position = new Vector3(_initialPosition.x, MathF.Sin(Time.time * _frequency) * _amplitude, _initialPosition.z);
//        }
//    }
//}
