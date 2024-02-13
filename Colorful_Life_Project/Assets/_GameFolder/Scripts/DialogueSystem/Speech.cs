using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Speech 1", menuName = "Dialogue/Speech", order = 0)]
public class Speech : ScriptableObject
{
    public string Speaker;
    [TextArea]
    public string Words;
    public Speech NextSpeech;
    public string GameObjectToChange;
    public List<string> DialogueGameObjectToDisable;
    public bool IsDisablelingOnEnd;

}
