using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatObject : MonoBehaviour
{
    [SerializeField]
    private Transform _transform;
    [SerializeField]
    private float _plusYPosition;
    [SerializeField]
    private float _minusYPosition;
    [SerializeField]
    private bool _isGoingToStartLocation;
    [SerializeField]
    private Transform _startLocation;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGoingToStartLocation &&
            !_transform.position.Equals(_startLocation))
        {

        }
    }
}
