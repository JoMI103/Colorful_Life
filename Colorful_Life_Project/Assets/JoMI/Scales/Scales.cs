using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scales : MonoBehaviour
{
   //5 a 7 height
   //-25 a 25 graus

    [SerializeField] private SphereCollider _leftDeliverArea, _rightDeliverArea;

    [SerializeField] private Transform _pivotPoint;
    [SerializeField] private Transform _scaleHeight;

    [SerializeField] private Transform _leftScale, _rightScale;
    [SerializeField] private Transform _leftPoint, _rightPoint;

    [SerializeField, Header("Masks")]
    MasksSO _masksSO;
    [SerializeField] private Transform _masksContainer;
    [SerializeField] private GameObject _maskPrefab;
    [SerializeField] private Masks[] masks;

    [SerializeField] LayerMask _masksLayerMask;

    [SerializeField] int nMasks;
    Masks[] _leftMasks;
    Masks[] _rightMasks;
  
    void Start()
    {
        _leftMasks = new Masks[nMasks * 2];
        _rightMasks = new Masks[nMasks * 2];
    }


    (float,float) calcScaleThings()
    {
        float height = 0;
        float balance = 0;

        int nMasks = 0;

        for (int i = 0; i < nMasks; i++)
        {
            if (!_leftMasks[i] && !_rightMasks[i]) continue;
            if(_leftMasks[i] && _rightMasks[i])
            {
                height += 2;
                continue;
            }

            if (_leftMasks[i]) balance--; else balance++;
            height++;
        }

        for (int i = nMasks; i < nMasks * 2; i++)
        {
            if (_leftMasks[i]) height++; balance--;
            if (_rightMasks[i]) height++; balance++;
        }

        return (height / (nMasks * 2), balance / nMasks + 0.5f);

    }
    void Update()
    {
        Collider[] leftColls = Physics.OverlapSphere(_leftDeliverArea.center + transform.position,
                                               _leftDeliverArea.radius,
                                               _masksLayerMask);
        Collider[] rightColls = Physics.OverlapSphere(_rightDeliverArea.center + transform.position,
                                               _rightDeliverArea.radius,
                                               _masksLayerMask);


        _leftScale.position = _leftPoint.position;
        _rightScale.position = _rightPoint.position;
    }


}
