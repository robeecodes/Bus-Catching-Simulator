using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepyBehaviour : MonoBehaviour, INPCBehavior
{
    private static readonly int Still = Animator.StringToHash("Still");
    [SerializeField] private Transform creepySpawnTarget;
    [SerializeField] private Camera playerCamera;

    // Handling of freaky behaviour
    [SerializeField] private GameObject torso;
    [SerializeField] private AudioSource crackSFX;
    [SerializeField] private AudioSource stepSFX;
    private Vector3 _initialTorsoPos;
    private Vector3 _previousTorsoPos;
    private float _moveThreshold = 1f;
    private float _timeBetweenCracks = 1f;
    private float _lastCrackTime;

    private NPCController _npcController;

    private CreepyStates _state;

    private float _creepyTimer;
    private readonly float _creepyTimerMax = 25f;

    private bool _noticed;
    private bool _isNight;

    private enum CreepyStates
    {
        Idle,
        Active
    }

    private void Start()
    {
        _initialTorsoPos = torso.transform.position;
        _previousTorsoPos = _initialTorsoPos;

        var skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        skinnedMeshRenderer.updateWhenOffscreen = true;
    }


    public void HandleState()
    {
        bool inView = IsInMainCameraView.IsInView(_npcController.transform.position, playerCamera);
        if (_state == CreepyStates.Active)
        {
            // Check if it has become nighttime
            if (!_isNight)
            {
                _isNight = GameManager.Instance.lightingManager.TimeOfDay >= 21 ||
                           GameManager.Instance.lightingManager.TimeOfDay <= 3;
                
                if (_isNight && stepSFX.isPlaying)
                {
                    stepSFX.Stop();
                }
            }
                
            if (_isNight)
            {
                CrackHeadToPlayer();
                float distanceMoved = Vector3.Distance(torso.transform.position, _previousTorsoPos);
            
                if (distanceMoved > _moveThreshold && (Time.time - _lastCrackTime) > _timeBetweenCracks)
                {
                    PlayCrackSound();
                    _lastCrackTime = Time.time;
                    _previousTorsoPos = torso.transform.position;
                }
            }
            
            if (!inView && _noticed)
            {
                transform.position = creepySpawnTarget.position;
                transform.LookAt(_npcController.playerTransform);
                
                transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
                
                stepSFX.panStereo = Random.Range(-0.36f, 0.36f);
                stepSFX.pitch = Random.Range(0.8f, 1.4f);
                stepSFX.PlayOneShot(stepSFX.clip);
                _noticed = false;
            }
        }

        if (inView && !_noticed)
        {
            _noticed = true;
        }
    }

    private void CrackHeadToPlayer()
    {
        var torsoPos = new Vector3(creepySpawnTarget.position.x + 0.25f, 0.5f, creepySpawnTarget.position.z - 0.5f);
        torso.transform.position =
            Vector3.Lerp(
                torso.transform.position,
                torsoPos,
                Time.deltaTime * 10f);
        torso.transform.LookAt(_npcController.playerTransform);
    }

    private void PlayCrackSound()
    {
        if (!crackSFX) return;

        crackSFX.pitch = Random.Range(0.8f, 1.4f);
        crackSFX.PlayOneShot(crackSFX.clip);
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
        }
    }
}