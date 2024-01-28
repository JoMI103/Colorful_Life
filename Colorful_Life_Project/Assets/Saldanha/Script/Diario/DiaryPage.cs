using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pages", menuName = "Book/Pages")]
public class DiaryPage : ScriptableObject
{
    public int _id;
    public string _type;
    public string _title;

    [TextArea(3, 10)]
    public string _content;
}


