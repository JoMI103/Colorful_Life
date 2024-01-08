using UnityEngine;

public interface IHittable {
    string Name { get; }
    GameObject coisoQueAtacou { get; } //dar um nome melhor
    void Hit(GameObject coisoQueAtacou,Vector3 direction , Vector3 inpactPos ,int damage); //dar um nome melhor
    void Killed();
}