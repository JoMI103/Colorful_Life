using UnityEngine;

public class SimpleBox : MonoBehaviour, IHittable, IGrabbable
{
    //[SerializeField] private int weight;

    private Rigidbody _rb;

    private void Start()
    {
        Offset = _offset;
        leftHandPos = leftT; rightHandPos = rightT;
        _rb = GetComponent<Rigidbody>();
    }


    #region Interfaces
   // public string Name { get; set; }
   // public GameObject coisoQueAtacou { get; set; }

    public Vector2 _offset;
    public Vector2 Offset { get; set; }
    public Transform leftHandPos { get; set; }
    public Transform rightHandPos { get; set; }
    public Transform leftT, rightT;

    public void Hit(GameObject coisoQueAtacou, Vector3 direction,Vector3 inpactPos, int damage, bool impact = false)
    {

        _rb.AddForceAtPosition(direction, inpactPos, ForceMode.Impulse);
        //_rb.AddForce(direction * 10, ForceMode.Impulse);

    }

 

    public void Killed() => Destroy(this.gameObject);

    public (Quaternion, Quaternion, Vector2) Grab()
    {
        _rb.useGravity = false;
        return (leftHandPos.rotation, rightHandPos.rotation,Offset);
    }

    public void UnGrab()
    {
        _rb.useGravity = true;
    }
    #endregion
}
