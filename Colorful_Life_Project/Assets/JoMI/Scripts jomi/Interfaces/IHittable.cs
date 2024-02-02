using UnityEngine;

public interface IHittable {

    void Hit(GameObject hittedBy, Vector3 hitDirection , Vector3 inpactPosition , int damage);
    void Killed();
}