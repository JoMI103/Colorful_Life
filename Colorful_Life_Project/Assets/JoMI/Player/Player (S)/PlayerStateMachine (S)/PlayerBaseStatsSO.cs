using UnityEngine;

[CreateAssetMenu(fileName = "Player Base Stats", menuName = "Player/Base Stats", order = 1)]
public class PlayerBaseStatsSO : ScriptableObject
{

    public float MovementSpeed = 10;
    public float RotationSpeed = 30;

    public float DashSpeedMultiplier = 1;
    public float DashDuration = 1;

    public float HittedImunityTime;
    public float HittedKnockbackDuration;

    [Header("Spell Stats")]
    public int ExplosionDmg;
    public float ExplosionForce;

    [Header("Despair Stats")]
    public float DespairMaxTime;
    public float DespairAniDuration;

    [Header("Scene Start needed")]
    public float MaxJumpHeight = 4;
    public float MaxJumpTime = 0.5f;

    public int MaxHP;
}