using Cinemachine;
using controller;
using UnityEngine;

/// <summary>
/// Autowires the PlayerController into the CinemachineVirtualCamera.
/// </summary>
public class CameraRigController : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        virtualCamera.Follow = FindObjectOfType<PlayerController>().transform;
    }
}