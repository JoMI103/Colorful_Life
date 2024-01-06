using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowSomething : MonoBehaviour
{
    public GameObject follow;
    private Vector3 offSet;

    private void Start() {
        offSet = this.transform.position - follow.transform.position;
    }

    private void Update() {
        this.transform.position = follow.transform.position + offSet;

        offSet.y += Input.mouseScrollDelta.y * Time.deltaTime *20;
    }
}
