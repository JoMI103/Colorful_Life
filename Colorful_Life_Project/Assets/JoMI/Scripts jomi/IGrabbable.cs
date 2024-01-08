using UnityEngine;

public interface IGrabbable 
{
    Vector2 Offset { get; set; }
    Transform leftHandPos { get; set; }
    Transform rightHandPos { get; set; }
    (Quaternion, Quaternion, Vector2 Offset) Grab();
    void UnGrab();
}
