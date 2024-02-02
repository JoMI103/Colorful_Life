using UnityEngine;

public interface IGrabbable 
{
    float Offset { get; set; }
    Transform leftHandPos { get; set; }
    Transform rightHandPos { get; set; }
    public GameObject GetGameObject { get; }

    void Grab();
    (Quaternion, Quaternion, float Offset) GetGrabablesData();
    void UnGrab();
}
