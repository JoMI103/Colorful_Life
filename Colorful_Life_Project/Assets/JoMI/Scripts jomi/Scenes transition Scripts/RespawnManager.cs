using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class RespawnManager : MonoBehaviour
{
    [SerializeField]  RespawnQuestionsSO _respawnQuestionsSO;

    [SerializeField] Button _yesButton, _noButton;
    [SerializeField] TextMeshProUGUI _questionText;
    [SerializeField] GameObject _deathPanel;

    public void PlayerKilled()
    {
        Invoke(nameof(KillUI), 2);
        
    }

    public void KillUI()
    {
        _deathPanel.SetActive(true);
        _yesButton.onClick.RemoveAllListeners();
        _noButton.onClick.RemoveAllListeners();



        Question _currentQuestion = _respawnQuestionsSO.questions[Random.Range(0, _respawnQuestionsSO.questions.Length)];

        if (_currentQuestion.CorrectAnswer)
        {

        //parte para adiconar a ui do butao quebrado para o nao
            _yesButton.onClick.AddListener(CorrectAnswer);
        }
        else
        {

        //parte para adiconar a ui do butao quebrado para o sim
            _noButton.onClick.AddListener(CorrectAnswer);
        }

        _questionText.text = _currentQuestion.QuestionText;
    }
    


    public void CorrectAnswer()
    {
        _deathPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}
