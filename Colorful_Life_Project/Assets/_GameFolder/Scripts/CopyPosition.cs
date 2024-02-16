using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{

    public Transform TargetTransform;
    public Transform TransformToCopy;
    public bool IsCopyingX = false;
    public bool IsCopyingY = false;
    public bool IsCopyingZ = false;
    public bool IsCopying = true;

    private void Awake()
    {
        if (TargetTransform == null) TargetTransform = gameObject.transform;
    }

    void Update()
    {
        Copy();
    }

    public void SetIsCopying (bool isCopying)
    {
        IsCopying = isCopying;
    }

    private void Copy ()
    {
        if (IsCopying && TargetTransform != null && TransformToCopy != null)
        {
            Vector3 finalPostion = TargetTransform.position;
            Vector3 positionToCopy = TransformToCopy.position;

            if (IsCopyingX) finalPostion.x = positionToCopy.x;
            if (IsCopyingY) finalPostion.y = positionToCopy.y;
            if (IsCopyingZ) finalPostion.z = positionToCopy.z;

            TargetTransform.position = finalPostion;
        }
    }
}
