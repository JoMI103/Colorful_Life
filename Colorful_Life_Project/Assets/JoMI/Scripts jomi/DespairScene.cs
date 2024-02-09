using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DespairScene : MonoBehaviour
{
    [SerializeField] private float velocity;

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 0, 1) * Time.deltaTime * velocity;
        velocity += Time.deltaTime * 1.5f;
    }
}
