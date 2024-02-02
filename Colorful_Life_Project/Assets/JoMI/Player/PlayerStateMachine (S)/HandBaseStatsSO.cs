using UnityEngine;

[CreateAssetMenu(fileName = "Hand Base Stats", menuName = "Player/Hand Base Stats", order = 1)]
public class HandBaseStatsSO : ScriptableObject
{
    [Header("Hand State Machine ctx")]
    public float MoveSpeed = 10;
    [Range(0.01f, 30f)]
    public float Func1 = 30;

    [Range(0.01f, 30f)] public float Func2V1;
    [Range(-1, 1)] public float Func2V2;


    [Range(-2, 2)] public float BodyInflunce;

    public float RotationSpeed;

    public float AnimationMovementSpeed;
    public float AnimationRotationSpeed;

    public float MaxPunchDistance;
    public float MinPunchDistance;
    public float punchChargTimeMult;




}
