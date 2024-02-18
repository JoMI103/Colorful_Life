using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Scales : MonoBehaviour
{
    [SerializeField] private GameObject _Door;

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





        for (int i = 0; i < nMasks * 2; i++)
        {
            if (_leftMasks[i]) height++;
            if (_rightMasks[i]) height++;
        }


        for (int i = 0; i < nMasks * 2; i++)
        {
            if (_leftMasks[i]) balance++; 
            if (_rightMasks[i]) balance--; 
        }

        return new Vector2(height / (nMasks * 2), (balance / nMasks / 2));

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
                if (mask.CorrectAnswer) {
                    _leftMasks[index] = mask.MaskScript;
                    mask.MaskPlaced = true;
                    scalesChanged =  true;
                    continue;
                }
                else
                {
                    _leftMasks[nMasks + index] = mask.MaskScript;
                    scalesChanged =  true;
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
                if (mask.CorrectAnswer)
                {
                    scalesChanged =  true;
                    _rightMasks[nMasks + index] = mask.MaskScript;
                    mask.MaskPlaced = true;
                    continue;
                }
                else
                {
                    scalesChanged =  true;
                    _rightMasks[index] = mask.MaskScript;
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
                yield return 1;
                updateMasksPos();
                if(_targetHeightBalance.x > 0.99)
                {
                        if(check()) Yay(); else Ohh();
                    
                   
                }
                
           
                
            }
            yield return new WaitForSeconds(0.5f);
        }
    }


    void Update()
    {

        float height = Mathf.Lerp(7,5, _targetHeightBalance.x);
        float degrees = Mathf.Lerp(-100, 100, _targetHeightBalance.y + 0.5f);
        _scaleHeight.localPosition =  new Vector3(_scaleHeight.localPosition.x, height, _scaleHeight.localPosition.z);
        _pivotPoint.localRotation = quaternion.Euler(new Vector3(0, degrees, 0));
    


        _leftScale.position = _leftPoint.position;
        _rightScale.position = _rightPoint.position;
    }

    private bool check()
    {
  

        for (int i = 0; i < nMasks; i++)
        {
            if (!_leftMasks[i]) return false;
            if (!_rightMasks[i]) return false;
        }

        return true;

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
        Destroy(_Door.gameObject);
        Debug.Log("YAAAAAAY");
    }

    private void Ohh()
    {
        Debug.Log("OHHHH");
        int aux = 0;
        for (int i = 0; i < nMasks * 2; i++)
        {
            Debug.Log(aux);
            if (_leftMasks[i]) { _leftMasks[i].MaskPlaced = false; _leftMasks[i].TargetPos = _descartedScales.position + scalesPosDescart[aux]; aux++; }
            if (_rightMasks[i]) { _rightMasks[i].MaskPlaced = false; _rightMasks[i].TargetPos = _descartedScales.position + scalesPosDescart[aux]; aux++; }
        }

        _leftMasks = new Masks[nMasks * 2];
        _rightMasks = new Masks[nMasks * 2];
        updateMasksPos();
    }

   
}
