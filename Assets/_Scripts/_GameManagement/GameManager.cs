using System.Collections;
using System.Collections.Generic;
using Sydewa;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private DynamicMoveProvider moveLocomotion;
    [SerializeField] private TeleportationProvider teleportLocomotion;
    [SerializeField] private XRRayInteractor teleportInteractor;

    [SerializeField] private LightingManager lightingManager;
    private float _dayStartTime = 18.5f;
    private float _dayEndTime = 3.0f;
    private float _dayLength;

    private float _fogMinDensity = 0.04f;
    private float _fogMaxDensity = 1f;

    private void Start()
    {
        _dayLength =
            _dayEndTime < _dayStartTime ? (_dayEndTime + 24) - _dayStartTime : _dayEndTime - _dayStartTime;
    }

    private void Update()
    {
        GradualFogification.IncreaseDensity(lightingManager.TimeOfDay, _dayStartTime, _dayEndTime, _fogMinDensity,
            _fogMaxDensity);
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
}