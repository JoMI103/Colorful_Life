using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MasksGroup", menuName = "MasksGroup", order = 1)]
public class MasksSO : MonoBehaviour
{
    public MaskGroup[] MasksGroups;
}

[Serializable]
public struct MaskGroup
{
    public string FirstText;
    public bool ComplementaryText;
}


