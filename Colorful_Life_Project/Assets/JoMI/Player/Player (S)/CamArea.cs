using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamArea : MonoBehaviour
{
    private CameraManager _camManager;

    [SerializeField] private bool fixedRotation;

    [SerializeField] private float _camTranslationVelocityMultiplier;
    [SerializeField] private float _camRotationVelocityMultiplier;
    [SerializeField] private bool _instant;

    public float CamTranslationVelocityMultiplier { get => _camTranslationVelocityMultiplier; }
    public float CamRotationVelocityMultiplier { get => _camRotationVelocityMultiplier; }
    public bool Instant { get => _instant;  }
    public bool FixedRotation { get => fixedRotation;  }

    [SerializeField] private Transform[] CamPositions;


    public Vector3 GetFinalPosition(Vector3 PlayerPos)
    {
        if (CamPositions.Length == 1) return CamPositions[0].position;
        if (CamPositions.Length == 0) return Vector3.zero;
        return Vector3.zero;
        Vector3 finalPos;

        foreach (Transform v in CamPositions)
        {
            
        }


 
    }
    
    public Vector3 GetFinalForward(Vector3 PlayerPos)
    {
        if (CamPositions.Length == 1) return CamPositions[0].forward;
        if (CamPositions.Length == 0) return Vector3.forward;
        return Vector3.zero;
        Vector3 finalPos;

        foreach (Transform v in CamPositions)
        {
            
        }


 
    }
    private void OnTriggerEnter(Collider other)
    {
        _camManager = other.GetComponent<PlayerContext>()?.GetMainCameraFromPlayerContext?.GetComponent<CameraManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(_camManager != null) if (!_camManager.CurrentCamArea) _camManager.CurrentCamArea = this; 
    }

    private void OnTriggerExit(Collider other)
    {
        if (_camManager != null) if (_camManager.CurrentCamArea == this) _camManager.CurrentCamArea = null; 
        
    }
}
