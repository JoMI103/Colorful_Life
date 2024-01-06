using UnityEngine;
using jomi.CharController3D;

public class PlayerHands : MonoBehaviour
{
    [SerializeField] private BoxCollider coll;
    [SerializeField] private Hand _LeftHand, _RightHand;

    MonoBehaviour currentIGrabbable;
    GameObject _grabbedObject;


    private void Update()
    {


        if (_grabbedObject == null)
        {
            _LeftHand.updateTarget(debugleftPos.position, Quaternion.identity);
            _RightHand.updateTarget(debugrightPos.position, Quaternion.identity);

            _LeftHand.updateTarget(transform.localRotation * _HandDefaultPosition + transform.position, transform.localRotation);
            _RightHand.updateTarget(transform.localRotation * new Vector3(-_HandDefaultPosition.x, _HandDefaultPosition.y, _HandDefaultPosition.z) + transform.position, transform.localRotation);
        }


        if (Input.GetKeyDown(KeyCode.T))
        {
            if (_grabbedObject) { _grabbedObject = null; } else

            if (currentIGrabbable != null)
            {
                _grabbedObject = currentIGrabbable.gameObject;
                (Transform, Transform) coiso = (currentIGrabbable as IGrabbable).Grab();
                _LeftHand.updateTarget(coiso.Item1.position, coiso.Item1.rotation,true);
                _RightHand.updateTarget(coiso.Item2.position, coiso.Item2.rotation,true);
            }
            
        }

        if (_grabbedObject && !_LeftHand.grabbing && !_RightHand.grabbing)
        {
            _grabbedObject.transform.position = _RightHand.transform.position + (_LeftHand.transform.position - _RightHand.transform.position) / 2;
            _LeftHand.updateTarget(transform.localRotation * _HandDefaultPosition + transform.position, transform.localRotation,false);
            _RightHand.updateTarget(transform.localRotation * new Vector3(-_HandDefaultPosition.x, _HandDefaultPosition.y, _HandDefaultPosition.z) + transform.position, transform.localRotation);
        }

        MonoBehaviour hitedGrabbable = null;

        Collider[] colliders = Physics.OverlapBox(transform.position + transform.rotation * coll.center, coll.size);

        foreach (Collider c in colliders)
        {
            MonoBehaviour[] allScripts = c.gameObject.GetComponentsInChildren<MonoBehaviour>();
            for (int i = 0; i < allScripts.Length; i++)
            {
                if (allScripts[i] is IGrabbable)
                    hitedGrabbable = allScripts[i];
            }
        }

        if (hitedGrabbable != currentIGrabbable)
        {
            currentIGrabbable = hitedGrabbable;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(transform.position + transform.rotation * coll.center , coll.size);
    }


    [SerializeField] private Vector3 _HandDefaultPosition;
    [SerializeField] private bool _DrawGizmos;
    [SerializeField] private Mesh _HandDebugMesh;
    [SerializeField, Range(0,2)] private float _debugScale;

    [SerializeField] Transform debugleftPos, debugrightPos;


    public void OnDrawGizmos() {
        
        

        if (!_DrawGizmos) return;
        Gizmos.DrawMesh(_HandDebugMesh, transform.localRotation * _HandDefaultPosition + transform.position, Quaternion.identity,Vector3.one * _debugScale);
        Gizmos.DrawMesh(_HandDebugMesh, transform.localRotation * new Vector3(-_HandDefaultPosition.x,_HandDefaultPosition.y,_HandDefaultPosition.z) + transform.position, Quaternion.identity,Vector3.one * _debugScale);
    }
}
