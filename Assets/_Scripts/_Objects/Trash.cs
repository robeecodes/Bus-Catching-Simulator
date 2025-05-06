using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Trash : MonoBehaviour
{
    public bool wasThrown = false;

    private NavMeshAgent _agent;
    private Rigidbody _rb;
    private Transform _player;
    private AudioSource _trashSound;

    private bool _isAlive = false;
    private bool _isHovered = false;

    private float _changeDestinationTimer = 0f;
    private float _leapTimer = 0f;

    [Header("Movement Settings")] [SerializeField]
    private float scuttleSpeed = 1.5f;

    [SerializeField] private float panicSpeed = 6f;
    [SerializeField] private float destinationChangeRate = 2f;
    [SerializeField] private float leapForce = 3f;
    [SerializeField] private float leapInterval = 1.5f;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();
        _trashSound = GetComponent<AudioSource>();
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;

        GameManager.Instance.OnTimeChanged += OnTimeChanged;

        _isAlive = false;
        _agent.enabled = false;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnTimeChanged -= OnTimeChanged;
    }

    private void Update()
    {
        if (GameManager.Instance.lightingManager.TimeOfDay >= 21 && _agent.isOnNavMesh)
        {
            EnableAgent();
        }

        if (!_isAlive) return;

        _changeDestinationTimer -= Time.deltaTime;
        _leapTimer -= Time.deltaTime;

        if (_changeDestinationTimer <= 0f)
        {
            PickNewDestination();
            _changeDestinationTimer = Random.Range(destinationChangeRate * 0.5f, destinationChangeRate * 1.5f);
        }

        if (_leapTimer <= 0f)
        {
            Leap();
            _leapTimer = Random.Range(leapInterval * 0.7f, leapInterval * 5.3f);
        }

        if (_agent.isActiveAndEnabled) return;
        if (!_agent.isOnNavMesh) return;
        if (!NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 2f, NavMesh.AllAreas)) return;
        _agent.enabled = true;
        _agent.Warp(hit.position);
    }

    private void PickNewDestination()
    {
        if (!_agent.isActiveAndEnabled || !_agent.isOnNavMesh)
            return; // Agent not ready, skip

        Vector3 randomDirection = Random.insideUnitSphere * 3f;
        randomDirection.y = 0f;

        if (_isHovered)
        {
            randomDirection = (transform.position - _player.position).normalized * 5f;
            _agent.speed = panicSpeed;
        }
        else if (_player && Random.value < 0.9f)
        {
            randomDirection = (_player.position - transform.position).normalized * 5f;
            _agent.speed = scuttleSpeed;
        }
        else
        {
            _agent.speed = scuttleSpeed;
        }

        Vector3 target = transform.position + randomDirection;

        if (NavMesh.SamplePosition(target, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            _agent.SetDestination(hit.position);
            _changeDestinationTimer = Random.Range(1f, 3f);
        }
        else
        {
            _isAlive = false;
            _agent.enabled = false;
        }
    }


    private void Leap()
    {
        if (_rb)
        {
            Vector3 leapDirection = Random.insideUnitSphere;
            leapDirection.y = 1f;
            _rb.AddForce(leapDirection * leapForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_trashSound)
        {
            _trashSound.pitch = Random.Range(0.7f, 1.1f);
            _trashSound.PlayOneShot(_trashSound.clip);
        }
    }

    private void OnTimeChanged(int time)
    {
        if (time >= 21)
        {
            EnableAgent();
        }
    }

    public void SetHovered()
    {
        _isHovered = true;
        if (_isAlive)
        {
            _changeDestinationTimer = 0f;
            _agent.speed = panicSpeed;
            PickNewDestination();
        }
    }


    public void UnsetHovered()
    {
        _isHovered = false;
        if (_isAlive)
        {
            _agent.speed = scuttleSpeed;
        }
    }

    public void EnableAgent()
    {
        _isAlive = true;
        _agent.enabled = true;
    }

    public void DisableAgent()
    {
        _isAlive = false;
        _agent.enabled = false;
    }
}