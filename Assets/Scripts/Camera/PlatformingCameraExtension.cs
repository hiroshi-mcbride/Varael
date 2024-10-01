using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

[SaveDuringPlay][AddComponentMenu("")]
public class PlatformingCameraExtension : CinemachineExtension
{
    public PlatformMovement playerMovement;
    public GroundCheck playerGroundCheck;
    private bool wasMoving;

    private PlayerCameraInputAsset inputAsset;
    
    private CinemachineVirtualCamera vc;
    private CinemachineComposer c;
    private CinemachineFramingTransposer ft;

    [Range(.0f,2.0f)] public float groundedDeadzoneHeight;
    [Range(.0f, 2.0f)] public float jumpingDeadzoneHeight;

    protected override void Awake()
    {
        base.Awake();
        //inputAsset = new PlayerCameraInputAsset();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        //inputAsset.Enable();
    }

    private void OnDisable()
    {
        //inputAsset.Disable();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase _vcam, CinemachineCore.Stage _stage, ref CameraState _state, float _deltaTime)
    {
        if (vc == null || c == null)
        {
            vc = _vcam as CinemachineVirtualCamera;
            if (vc == null)
            {
                Debug.Log("PlatformingCameraExtension: No virtual camera found");
                return;
            }
            c = vc.GetCinemachineComponent<CinemachineComposer>();
            ft = vc.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
        
        bool isMoving = playerMovement.IsMoving();
        if (_stage == CinemachineCore.Stage.Body)
        {
            if (playerGroundCheck.isGrounded)
            {
                ft.m_DeadZoneHeight = Mathf.Lerp(ft.m_DeadZoneHeight, groundedDeadzoneHeight, .1f);
            }
            else
            {
                ft.m_DeadZoneHeight = Mathf.Lerp(ft.m_DeadZoneHeight, jumpingDeadzoneHeight, .1f);
            }
        }
        if (_stage == CinemachineCore.Stage.Aim)
        {
            
        }
        wasMoving = isMoving;
    }

}
