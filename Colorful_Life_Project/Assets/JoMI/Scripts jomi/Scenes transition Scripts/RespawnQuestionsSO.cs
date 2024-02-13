using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Questions", menuName = "Questions", order = 1)]
public class RespawnQuestionsSO : ScriptableObject
{

    public Question[] questions;
    

}

[Serializable]
public struct Question
{
    public string QuestionText;
    public bool CorrectAnswer;
}