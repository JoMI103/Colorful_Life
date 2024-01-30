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

    public float _offset;
    public float Offset { get; set; }
    public Transform leftHandPos { get; set; }
    public Transform rightHandPos { get; set; }

    public GameObject GetGameObject { get => this.gameObject; }

    public Transform leftT, rightT;

    public void Hit(GameObject coisoQueAtacou, Vector3 direction,Vector3 inpactPos, int damage, bool impact = false)
    {

        _rb.AddForceAtPosition(direction, inpactPos, ForceMode.Impulse);
        //_rb.AddForce(direction * 10, ForceMode.Impulse);

    }

 

    public void Killed() => Destroy(this.gameObject);



    public void UnGrab()
    {
        _rb.useGravity = true;
    }


    public (Quaternion, Quaternion, float Offset) GetGrabablesData()
    {
        return (leftHandPos.rotation, rightHandPos.rotation,Offset);
    }

    void IGrabbable.Grab()
    {
        _rb.useGravity = false;
    }
    #endregion
}
