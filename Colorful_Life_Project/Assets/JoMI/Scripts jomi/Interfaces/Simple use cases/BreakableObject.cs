using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour, IHittable
{
    [SerializeField] private GameObject _InstantiatedObjectIfDestroyed;
    [SerializeField] private int _maxHP;

    public void Hit(GameObject hittedBy, Vector3 hitDirection, Vector3 inpactPosition, int damage)
    {
        _maxHP -= damage;
        if (_maxHP <= 0) Killed();
    }

    public void Killed()
    {
        if (_InstantiatedObjectIfDestroyed) Instantiate(_InstantiatedObjectIfDestroyed, transform.position,Quaternion.identity);
        Destroy(this.gameObject);
    }
}
