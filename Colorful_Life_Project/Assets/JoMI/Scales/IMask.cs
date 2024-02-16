using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IMask 
{
    public int GroupMask { get; }
    public Masks MaskScript { get; }
    public bool MaskPlaced { get; set; }
    public Vector3 TargetPos { get; set; }

}
