using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Camera _camera; 

    [SerializeField] GameObject _playerGameObject;

   
    [SerializeField] private Vector3 _playerPosOffSet;
    [SerializeField] private Vector3 _cameraToPlayerOffSet;

    [SerializeField] private float _camTranslationVelocity;
    [SerializeField] private float _camRotationVelocity;

    private CamArea _currentCamArea;


    private bool lastCamWasInstant;


    private Vector3 PlayerPosWithOffSet { get => _playerGameObject.transform.position + _playerPosOffSet; }
    private Vector3 DefaulCameraPos { get => _cameraToPlayerOffSet + PlayerPosWithOffSet; }
    private Vector3 ForwardToPlayer { get => (PlayerPosWithOffSet - transform.position).normalized; }
    private Vector3 DefaultForwardToPlayer { get => (PlayerPosWithOffSet - DefaulCameraPos).normalized; }
    private float DefaultCameraTranslationVelocity { get => Time.deltaTime * _camTranslationVelocity * 10; }
    private float DefaultCameraRotationVelocity { get => Time.deltaTime * _camRotationVelocity * 3.14f; }
    public CamArea CurrentCamArea { get => _currentCamArea; set => _currentCamArea = value; }
    public Vector3 CameraToPlayerOffSet { get => _cameraToPlayerOffSet; set => _cameraToPlayerOffSet = value; }
    public Camera Camera { get => _camera; }

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        lastCamWasInstant = true;
    }


    void Update()
    {
        if (_currentCamArea) CamAreaMode(); else DefaultMode(); 
    }

    private void CamAreaMode()
    {
        if(_currentCamArea.Instant)
        {
            lastCamWasInstant = true;
            transform.position = _currentCamArea.GetFinalPosition(PlayerPosWithOffSet);
            transform.forward = (_currentCamArea.FixedRotation) ? _currentCamArea.GetFinalForward(PlayerPosWithOffSet) : ForwardToPlayer;
        }
        else
        {
            lastCamWasInstant = false;
            transform.position = Vector3.MoveTowards(transform.position,
                                                     _currentCamArea.GetFinalPosition(PlayerPosWithOffSet),
                                                     DefaultCameraTranslationVelocity * _currentCamArea.CamTranslationVelocityMultiplier);

            Vector3 finalForawrd = (_currentCamArea.FixedRotation) ? _currentCamArea.GetFinalForward(PlayerPosWithOffSet) : ForwardToPlayer;
            transform.forward = Vector3.RotateTowards(transform.forward, finalForawrd,DefaultCameraRotationVelocity * _currentCamArea.CamRotationVelocityMultiplier, 360);
        }
    }

    [ContextMenu("aux")]
    private void DefaultMode()
    {
        if (lastCamWasInstant)
        {
            transform.position = DefaulCameraPos;
            transform.forward = ForwardToPlayer;
        }
        else
        {

            transform.position = Vector3.MoveTowards(transform.position, DefaulCameraPos, DefaultCameraTranslationVelocity);
            transform.forward = Vector3.RotateTowards(transform.forward, DefaultForwardToPlayer, DefaultCameraRotationVelocity ,360); 
        }
        
    }

#if UNITY_EDITOR
    [SerializeField] private bool _debug;

    private void OnDrawGizmosSelected()
    {
        if (!_debug) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(DefaulCameraPos, 0.5f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(PlayerPosWithOffSet, 0.2f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(DefaulCameraPos + ForwardToPlayer, 0.25f);
      
    }
#endif
}
