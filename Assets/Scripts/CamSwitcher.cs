using UnityEngine;
using Unity.Cinemachine;

public class CamSwitcher : MonoBehaviour
{
    public CinemachineCamera fpsCam;
    public CinemachineCamera tpsCam;

    private bool isFPS = false;

    public void ToggleCameraMode()
    {
        isFPS = !isFPS;
        fpsCam.Priority = isFPS ? 20 : 10;
        tpsCam.Priority = isFPS ? 10 : 20;
    }
}