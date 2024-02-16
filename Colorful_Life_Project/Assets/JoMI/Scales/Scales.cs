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

     LayerMask _masksLayerMask;

    [SerializeField] int nMasks;

    [SerializeField, Header("Tem que ter nMask * 2")] Vector3[] scalesPosInPlate;
    [SerializeField] Transform _leftScalePlate;
    [SerializeField] Transform _rightScalePlate;

    [SerializeField, Header("Tem que ter nMask * 2")] Vector3[] scalesPosDescart;
    [SerializeField] Transform _descartedScales;

    Masks[] _leftMasks;
    Masks[] _rightMasks;

    [SerializeField] Vector2 _targetHeightBalance;

    void Start()
    {
        _leftMasks = new Masks[nMasks * 2];
        _rightMasks = new Masks[nMasks * 2];
        StartCoroutine(checkPuzzle());
    }


    Vector2 CalcScaleThings()
    {
        float height = 0;
        float balance = 0;


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

        return new Vector2(height / (nMasks * 2), balance / nMasks + 0.5f);

    }

    bool CheckDeliveryZones()
    {
        bool scalesChanged = false;

        Collider[] leftColls = Physics.OverlapSphere(_leftDeliverArea.center  + _leftDeliverArea.transform.position,
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
                    continue;
                }
                if (_leftMasks[nMasks + index] == null)
                {
                    _leftMasks[nMasks + index] = mask.MaskScript;
                    scalesChanged = true;
                    mask.MaskPlaced = true;
                }
            }
        }

        Collider[] rightColls = Physics.OverlapSphere(_rightDeliverArea.center  + _rightDeliverArea.transform.position,
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
                    continue;
                }
                if (_rightMasks[nMasks + index ] == null)
                {
                    _rightMasks[nMasks + index] = mask.MaskScript;
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
            Debug.LogError("CARALHO1");
                _targetHeightBalance = CalcScaleThings();
                yield return 1;
                updateMasksPos();
                if(_targetHeightBalance.x > 0.99)
                {
                    Debug.LogError("CARALHO2");
                    if (_targetHeightBalance.y > 0.49 && _targetHeightBalance.y < 0.51) { Yay(); }
                    else Ohh();
                }
                
           
                
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
        Debug.Log("OHHHH");
        int aux = 0;
        for (int i = 0; i < nMasks * 2; i++)
        {
            if (_leftMasks[i]) { _leftMasks[i].MaskPlaced = false; _leftMasks[i].TargetPos = _descartedScales.position + scalesPosDescart[aux]; aux++; }
            if (_rightMasks[i]) { _leftMasks[i].MaskPlaced = false; _rightMasks[i].TargetPos = _descartedScales.position + scalesPosDescart[aux]; aux++; }
        }

        _leftMasks = new Masks[nMasks * 2];
        _rightMasks = new Masks[nMasks * 2];
    }

}
