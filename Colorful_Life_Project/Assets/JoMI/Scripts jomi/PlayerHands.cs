using UnityEngine;

public class PlayerHands : MonoBehaviour
{
    [SerializeField] private Hand _LeftHand, _RightHand;



    private void Update()
    {
        _LeftHand.updateTarget(transform.localRotation * _HandDefaultPosition + transform.position, transform.localRotation, transform.position);
        _RightHand.updateTarget(transform.localRotation * new Vector3(-_HandDefaultPosition.x, _HandDefaultPosition.y, _HandDefaultPosition.z) + transform.position,transform.localRotation, transform.position);
    }



    [SerializeField] private Vector3 _HandDefaultPosition;
    [SerializeField] private bool _DrawGizmos;
    [SerializeField] private Mesh _HandDebugMesh;
    [SerializeField, Range(0,2)] private float _debugScale;


    public void OnDrawGizmos() {
        if(!_DrawGizmos) return;

        Gizmos.DrawMesh(_HandDebugMesh, transform.localRotation * _HandDefaultPosition + transform.position, Quaternion.identity,Vector3.one * _debugScale);
        Gizmos.DrawMesh(_HandDebugMesh, transform.localRotation * new Vector3(-_HandDefaultPosition.x,_HandDefaultPosition.y,_HandDefaultPosition.z) + transform.position, Quaternion.identity,Vector3.one * _debugScale);
    }
}
