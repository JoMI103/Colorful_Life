using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image foreGroundImage, backGroundImage;
    public Vector3 offset;

    public Transform _target;
    Camera _mainCamera;

    public void SetHealthBar(Camera mainCamera, Transform target)
    {
        _mainCamera = mainCamera;
        _target = target;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 direction = (_target.position - _mainCamera.transform.position).normalized;
        bool isBehind = Vector3.Dot(direction, _mainCamera.transform.forward) <= 0.0f;
        foreGroundImage.enabled = !isBehind;
        backGroundImage.enabled = !isBehind;
        transform.position = _mainCamera.WorldToScreenPoint(_target.position + offset);
    }

    public void SetHealthBarPercentage(float percentage)
    {
        Debug.Log(percentage);
        foreGroundImage.fillAmount = percentage;
    }
}
