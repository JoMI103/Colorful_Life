using System;
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

    [SerializeField, Header("Tem que ter nMask * 2")] Vector3[] scalesPosInPlate;
    [SerializeField] Transform _leftScalePlate;
    [SerializeField] Transform _rightScalePlate;

    [SerializeField, Header("Tem que ter nMask * 2")] Vector3[] scalesPosDescart;
    [SerializeField] Transform _descartedScales;

    Masks[] _leftMasks;
    Masks[] _rightMasks;

    (float, float) _targetHeightBalance;

    void Start()
    {
        _leftMasks = new Masks[nMasks * 2];
        _rightMasks = new Masks[nMasks * 2];
    }


    (float, float) CalcScaleThings()
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

    bool CheckDeliveryZones()
    {
        bool scalesChanged = false;

        Collider[] leftColls = Physics.OverlapSphere(_leftDeliverArea.center + transform.position,
                                               _leftDeliverArea.radius,
                                               _masksLayerMask);
        foreach (var coll in leftColls)
        {
            if (coll.TryGetComponent(out IMask mask))
            {
                if (mask.MaskPlaced) continue;

                int index = mask.GroupMask;
                if (_leftMasks[index] == null)
                {
                    _leftMasks[index] = mask.MaskScript;
                    scalesChanged = true;
                    mask.MaskPlaced = true;
                }
                if (_leftMasks[index * 2] == null)
                {
                    _leftMasks[index * 2] = mask.MaskScript;
                    scalesChanged = true;
                    mask.MaskPlaced = true;
                }
            }
        }

        Collider[] rightColls = Physics.OverlapSphere(_rightDeliverArea.center + transform.position,
                                               _rightDeliverArea.radius,
                                               _masksLayerMask);

        foreach (var coll in rightColls)
        {
            if (coll.TryGetComponent(out IMask mask))
            {
                if (mask.MaskPlaced) continue;

                int index = mask.GroupMask;
                if (_rightMasks[index] == null)
                {
                    _rightMasks[index] = mask.MaskScript;
                    scalesChanged = true;
                    mask.MaskPlaced = true;
                }
                if (_rightMasks[index * 2] == null)
                {
                    _rightMasks[index * 2] = mask.MaskScript;
                    scalesChanged = true;
                    mask.MaskPlaced = true;
                }
            }
        }
        return scalesChanged; 
    }


    IEnumerator checkPuzzle()
    {
        while(true)
        {
            if (CheckDeliveryZones())
            {
                _targetHeightBalance = CalcScaleThings();
                updateMasksPos();
                if (_targetHeightBalance.Item1 > 0.99 && _targetHeightBalance.Item2 > 0.49 && _targetHeightBalance.Item2 < 0.51) { Yay(); }
                else Ohh();
                
            }
            yield return new WaitForSeconds(0.5f);
        }
    }


    void Update()
    {
        


        _leftScale.position = _leftPoint.position;
        _rightScale.position = _rightPoint.position;
    }

    private void updateMasksPos()
    {
        int aux1 = 0;
        int aux2 = 0;
        for (int i = 0; i < nMasks * 2; i++)
        {
            if (_leftMasks[i]) { _leftMasks[i].TargetPos = _leftScalePlate.position + scalesPosInPlate[aux1]; aux1++; }
            if (_rightMasks[i]) { _rightMasks[i].TargetPos = _rightScalePlate.position + scalesPosInPlate[aux2]; aux2++; }
        }
    }

    private void Yay()
    {
        Debug.Log("YAAAAAAY");
    }

    private void Ohh()
    {
        int aux = 0;
        for (int i = 0; i < nMasks * 2; i++)
        {
            if (_leftMasks[i]) { _leftMasks[i].TargetPos = _descartedScales.position + scalesPosDescart[aux]; aux++; }
            if (_rightMasks[i]) { _rightMasks[i].TargetPos = _descartedScales.position + scalesPosDescart[aux]; aux++; }
        }
    }

}
