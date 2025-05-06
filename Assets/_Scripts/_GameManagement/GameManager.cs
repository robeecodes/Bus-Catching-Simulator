using System;
using Sydewa;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private DynamicMoveProvider moveLocomotion;
    [SerializeField] private TeleportationProvider teleportLocomotion;
    [SerializeField] private XRRayInteractor teleportInteractor;

    
    // Access time of day
    public LightingManager lightingManager;
    public event Action<int> OnTimeChanged;
    private int _lastReportedTime = -1;
    
    
    [SerializeField] private float fogStartTime = 19.5f;
    [SerializeField] private Material skyboxMat;
    [SerializeField] private float dayEndTime = 3.0f;

    [SerializeField] private float fogMinDensity = 0.04f;
    [SerializeField] private float fogMaxDensity = 0.6f;
    
    
    // Detect if screen is full of smoke
    public bool isScreenSmoke = false;
    
    private void Update()
    {
        GradualFogification.IncreaseDensity(lightingManager.TimeOfDay, fogStartTime, dayEndTime, fogMinDensity,
            fogMaxDensity, skyboxMat);
        
        OnTimeOfDayChanged();
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

    private void OnTimeOfDayChanged()
    {
        int currentTime = Mathf.FloorToInt(lightingManager.TimeOfDay);

        if (currentTime != _lastReportedTime)
        {
            _lastReportedTime = currentTime;
            OnTimeChanged?.Invoke(currentTime);
        }
    }
}