using UnityEngine;

public interface IGrabbable 
{
    float Offset { get; }
    (Vector3,Vector3) HandsRotations { get; }
    Vector3 Position { get; }

    void updatePosWithHandsPos(Vector3 middleHandsPos);
    void Grab();
    void UnGrab();
}
