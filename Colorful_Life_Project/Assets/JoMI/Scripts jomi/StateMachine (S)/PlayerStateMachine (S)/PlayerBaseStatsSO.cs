using UnityEngine;

[CreateAssetMenu(fileName = "Player Base Stats", menuName = "Player/Base Stats", order = 1)]
public class PlayerBaseStatsSO : ScriptableObject
{

    public float MovementSpeed = 10;
    public float RotationSpeed = 30;




    [Header("Scene Start needed")]
    public float MaxJumpHeight = 4;
    public float MaxJumpTime = 0.5f;
}