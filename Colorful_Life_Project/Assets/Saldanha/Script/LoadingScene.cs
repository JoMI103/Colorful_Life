using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private int _sceneName;
    [SerializeField] Image image;
    [SerializeField] private GameObject gameObjMenu;
    [SerializeField] private GameObject gameObjLoad;
    public void LoadScene()
    {
        gameObjLoad.SetActive(true);
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        gameObjMenu.SetActive(false);
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_sceneName);

        while (!asyncLoad.isDone)
        {
            if(image != null)
            {
                image.fillAmount = asyncLoad.progress;
            }
            yield return null;
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
