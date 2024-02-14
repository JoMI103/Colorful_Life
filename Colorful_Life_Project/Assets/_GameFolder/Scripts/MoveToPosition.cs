using UnityEngine;

public class MoveToPosition : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    public Transform DesiredPosition;
    [SerializeField]
    public Transform TargetTansform;
    public float TranslationVelocy;
    public bool IsMoving = true;
    [SerializeField]
    private float _distanteBetweenFinalLocationAndTarget = 2e-05f;

    private void Awake()
    {
        if (TargetTansform == null) TargetTansform = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMoving &&
            Vector3.Distance(TargetTansform.position, DesiredPosition.position) > _distanteBetweenFinalLocationAndTarget)
        {
            TargetTansform.position = Vector3.MoveTowards(TargetTansform.position, DesiredPosition.position, TranslationVelocy * Time.deltaTime);
        }
    }
}
