using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepyBehaviour : MonoBehaviour, INPCBehavior
{
    [SerializeField] private Transform creepySpawnTarget;
    [SerializeField] private Camera playerCamera;
    
    private NPCController _npcController;
    private enum CreepyStates
    {
        Idle,
        Active
    }
    
    private CreepyStates _state;
    
    private float _creepyTimer;
    private readonly float _creepyTimerMax = 25f;

    private bool _noticed = false;
    
    public void HandleState()
    {
        bool inView = IsInMainCameraView.IsInView(_npcController.transform.position, playerCamera);
        if (_state == CreepyStates.Active && !inView && _noticed)
        {
            transform.position = creepySpawnTarget.position;
            _noticed = false;
            Debug.Log("Creepy");
        }

        if (inView && !_noticed)
        {
            _noticed = true;
        }
    }

    public void Init(NPCController npcController)
    {
        _npcController = npcController;
        _state = CreepyStates.Idle;
        
        StartCoroutine(CreepyTimer());
    }

    public void Activate()
    {
        _state = CreepyStates.Active;
    }

private IEnumerator CreepyTimer()
{
    while (enabled)
    {
        _creepyTimer = Random.Range(5f, _creepyTimerMax);
        yield return new WaitForSeconds(_creepyTimer);
        
        _state = _state == CreepyStates.Idle ? CreepyStates.Active : CreepyStates.Idle;
        Debug.Log(_state);
    }
}
}