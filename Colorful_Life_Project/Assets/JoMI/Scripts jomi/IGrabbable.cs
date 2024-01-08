using UnityEngine;

public interface IGrabbable 
{
    Transform leftHandPos { get; set; }
    Transform rightHandPos { get; set; }
    (Transform, Transform) Grab();
    void UnGrab();
}
