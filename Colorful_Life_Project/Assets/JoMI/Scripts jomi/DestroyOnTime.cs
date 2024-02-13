using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTime : MonoBehaviour
{
    public float time;

    void Start() => Invoke(nameof(Kill), time);

    void Kill() => Destroy(this.gameObject);
}
