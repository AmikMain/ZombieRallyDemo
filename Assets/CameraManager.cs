using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] GameObject garageCamera;

    void Start()
    {
        GameManager.Instance.OnLapStart += DisableGarageCameraDelayed;
    }

    void OnDisable()
    {
        GameManager.Instance.OnLapStart -= DisableGarageCameraDelayed;
    }

    private void DisableGarageCameraDelayed()
    {
        Invoke(nameof(DisableGarageCamera) , 1f );
    }

    private void DisableGarageCamera()
    {
        garageCamera.SetActive(false);
    }
}
