using UnityEngine;

public class SimpleBox : MonoBehaviour, IHittable, IInteractable
{
    //[SerializeField] private int weight;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }


    #region Interfaces
    public string PromptMessage => "Pegar";
    public GameObject PlayerGO { get; set; }
    public string Name { get; set; }
    public GameObject coisoQueAtacou { get; set; }


    public void Hit(GameObject coisoQueAtacou, Vector3 inpactPos, int damage)
    {
        Vector3 direction = -(inpactPos - transform.position).normalized;
        _rb.AddForce(direction * 10, ForceMode.Impulse);
    }

    public void Interact(GameObject player)
    {
  
    }

    public void Killed() => Destroy(this.gameObject);
    #endregion
}
