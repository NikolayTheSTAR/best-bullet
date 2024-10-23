using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private ICameraFocusable defaultFocus;

    [Inject]
    private void Construct(ICameraFocusable defaultFocus)
    {
        this.defaultFocus = defaultFocus;
    }

    private void Start()
    {
        FocusTo(defaultFocus);
    }
    
    public void FocusTo(ICameraFocusable focus)
    {
        virtualCamera.m_Follow = focus?.transform;
    }
}

public interface ICameraFocusable
{
    Transform transform { get; }
}