using UnityEngine;

public class SimpleBox : MonoBehaviour, IHittable, IGrabbable
{
    //[SerializeField] private int weight;

    private Rigidbody _rb;

    private void Start()
    {
        leftHandPos = leftT; rightHandPos = rightT;
        _rb = GetComponent<Rigidbody>();
    }


    #region Interfaces
    public string Name { get; set; }
    public GameObject coisoQueAtacou { get; set; }

    public Transform leftHandPos { get; set; }
    public Transform rightHandPos { get; set; }
    public Transform leftT, rightT;

    public void Hit(GameObject coisoQueAtacou, Vector3 inpactPos, int damage)
    {
        Vector3 direction = -(inpactPos - transform.position).normalized;
        _rb.AddForce(direction * 10, ForceMode.Impulse);
    }

 

    public void Killed() => Destroy(this.gameObject);

    public (Transform, Transform) Grab()
    {
        _rb.useGravity = false;
        return (leftHandPos, rightHandPos);
    }

    public void UnGrab()
    {
        _rb.useGravity = true;
    }
    #endregion
}
