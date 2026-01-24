using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] GameObject garageCamera;

    void Start()
    {
        GameManager.Instance.OnLapStart += DisableGarageCameraDelayed;
        GameManager.Instance.OnLapReload += EnableGarageCameraDelayed;
    }

    void OnDisable()
    {
        GameManager.Instance.OnLapStart -= DisableGarageCameraDelayed;
        GameManager.Instance.OnLapReload -= EnableGarageCameraDelayed;
    }

    private void DisableGarageCameraDelayed()
    {
        Invoke(nameof(DisableGarageCamera) , 1f );
    }
    private void EnableGarageCameraDelayed()
    {
        Invoke(nameof(EnableGarageCamera) , .1f );
    }

    private void DisableGarageCamera()
    {
        garageCamera.SetActive(false);
    }

    private void EnableGarageCamera()
    {
        garageCamera.SetActive(true);
    }
}
