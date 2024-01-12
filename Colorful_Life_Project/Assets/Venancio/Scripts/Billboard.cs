using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public BillboardFuncion Function;
    private Camera _activeCamera;

    // Start is called before the first frame update
    void Start()
    {
        _activeCamera = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        switch (Function)
        {
            case BillboardFuncion.LookAtCamera:
                gameObject.transform.LookAt(_activeCamera.transform);
                break;

            case BillboardFuncion.OnlyYAxis:
                gameObject.transform.rotation = Quaternion.Euler(_activeCamera.transform.eulerAngles.x, 0f, 0f);
                break;
        }
    }
}

public enum BillboardFuncion
{
    LookAtCamera,
    OnlyYAxis
}
