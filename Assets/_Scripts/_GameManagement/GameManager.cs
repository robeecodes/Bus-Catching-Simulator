using System.Collections;
using System.Collections.Generic;
using Sydewa;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private DynamicMoveProvider moveLocomotion;
    [SerializeField] private TeleportationProvider teleportLocomotion;
    [SerializeField] private XRRayInteractor teleportInteractor;

    public LightingManager lightingManager;
    [SerializeField] private float fogStartTime = 21f;
    [SerializeField] private Material skyboxMat;
    private float _dayEndTime = 3.0f;

    [SerializeField] private float fogMinDensity = 0.04f;
    [SerializeField] private float fogMaxDensity = 0.6f;
    
    private void Update()
    {
        GradualFogification.IncreaseDensity(lightingManager.TimeOfDay, fogStartTime, _dayEndTime, fogMinDensity,
            fogMaxDensity, skyboxMat);
    }

    public void LockMovement()
    {
        moveLocomotion.enabled = false;
        teleportLocomotion.enabled = false;
        teleportInteractor.enabled = false;
    }

    public void UnlockMovement()
    {
        moveLocomotion.enabled = true;
        teleportLocomotion.enabled = true;
        teleportInteractor.enabled = true;
    }

    public void PauseTime()
    {
        lightingManager.IsDayCycleOn = false;
    }

    public void ResumeTime()
    {
        lightingManager.IsDayCycleOn = true;
    }
}