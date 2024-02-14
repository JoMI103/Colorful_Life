using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationUI : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    public void ScaleImage()
    {
        _gameObject.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    public void ScaleImageBack()
    {
        _gameObject.gameObject.transform.localScale = new Vector3(1, 1, 1);
    }
}
