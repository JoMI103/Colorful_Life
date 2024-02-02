using UnityEngine;
using jomi.CharController3D;
namespace old
{
    public class PlayerHands : MonoBehaviour
    {
        [SerializeField] private Hand _LeftHand, _RightHand;

        [SerializeField] private Vector2 _HandsYZBasePosition;
        [SerializeField] private float _handsBaseDistance;


        MonoBehaviour currentIGrabbable;
        GameObject _grabbedObject;

        private void Start()
        {

        }


        private void Update()
        {


            if (_grabbedObject == null)
            {
                // _LeftHand.updateTarget(gethandInBodyPos(false), transform.localRotation);
                // _RightHand.updateTarget(gethandInBodyPos(true), transform.localRotation);
            }


            if (Input.GetKeyDown(KeyCode.T)) GrabUngrabObject();
            {
                if (_grabbedObject) { _grabbedObject = null; }
                else

                if (currentIGrabbable != null)
                {
                    _grabbedObject = currentIGrabbable.gameObject;
                    //                (Transform, Transform) coiso = (currentIGrabbable as IGrabbable).Grab();
                    // _LeftHand.updateTarget(coiso.Item1.position, coiso.Item1.rotation, true);
                    // _RightHand.updateTarget(coiso.Item2.position, coiso.Item2.rotation, true);
                }

            }
            /*
            if (_grabbedObject && !_LeftHand.grabbing && !_RightHand.grabbing)
            {
                _grabbedObject.transform.position = _RightHand.transform.position + (_LeftHand.transform.position - _RightHand.transform.position) / 2;
               // _LeftHand.updateTarget(gethandInBodyPos(false), transform.localRotation);
               // _RightHand.updateTarget(gethandInBodyPos(true), transform.localRotation);
            }
            */
            FindGrabbableObjects();

        }

        Vector3 gethandInBodyPos(bool right = false)
        {
            float dist = right ? 1 : -1;
            return transform.localRotation * new Vector3(dist * _handsBaseDistance, _HandsYZBasePosition.x, _HandsYZBasePosition.y) + transform.position;
        }

        public void GrabUngrabObject()
        {
            if (_grabbedObject) _grabbedObject = null;
            //else
        }


        #region findObjects

        private void FindGrabbableObjects()
        {
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

        [SerializeField] private BoxCollider coll;

        private void OnDrawGizmosSelected()
        {
            // Gizmos.DrawCube(transform.position + transform.rotation * coll.center , coll.size);
        }
        #endregion



        #region debugGizmos
        [SerializeField] private bool _DrawGizmos;
        [SerializeField] private Mesh _HandDebugMesh;
        [SerializeField, Range(0, 2)] private float _debugScale;

        public void OnDrawGizmos()
        {



            if (!_DrawGizmos) return;
            Gizmos.DrawMesh(_HandDebugMesh, gethandInBodyPos(false), Quaternion.identity, Vector3.one * _debugScale);
            Gizmos.DrawMesh(_HandDebugMesh, gethandInBodyPos(true), Quaternion.identity, Vector3.one * _debugScale);
        }
        #endregion
    }
}