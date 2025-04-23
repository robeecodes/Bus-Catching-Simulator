using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.XR.Interaction.Toolkit;

public class NPCController : MonoBehaviour
{
    [SerializeField] private ScriptableObject[] NPCDataObjects;

    private static readonly int Throw = Animator.StringToHash("Throw");
    private NPCData _data;
    private Animator _animator;
    
    private Transform _target;
    
    [SerializeField] private Transform trashTarget;
    [SerializeField] private GameObject[] trashObjects;
    private GameObject _trash;
    [SerializeField] private float throwForce = 10f;

    private void Awake()
    {
        // _data = NPCDataObjects[Random.Range(0, NPCDataObjects.Length)] as NPCData;
        _data = NPCDataObjects[0] as NPCData;
        _animator = GetComponent<Animator>();

        Init();
    }

    private void Init()
    {
        if (_data.type == NPCType.Thrower)
        {
            var selectedTrashObject = trashObjects[Random.Range(0, trashObjects.Length)];
            _trash = Instantiate(selectedTrashObject, trashTarget.position, trashTarget.rotation, trashTarget.transform);
        
            // Ensure the Rigidbody starts as kinematic while held by NPC
            Rigidbody rb = _trash.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
        
            // Disable any XR Grab Interactable while NPC is holding it
            XRGrabInteractable grabbable = _trash.GetComponent<XRGrabInteractable>();
            if (grabbable != null)
            {
                grabbable.enabled = false;
            }
        }
    }

    private void Update()
    {
        HandleState();
    }
    
    public void Activate(Transform target)
    {
        _target = target;
        if (_data.type == NPCType.Thrower)
        {
            _data.state = "Throw";
        }
        
        _data.triggered = true;
    }

    private void HandleState()
    {
        switch (_data.state)
        {
            case "Throw":
                ThrowTrash(_target);
                break;
        }
    }

    private void ThrowTrash(Transform target)
    {
        transform.LookAt(target);
        _animator.SetTrigger(Throw);
        
        // Calculate direction to target
        Vector3 direction = (target.position - _trash.transform.position).normalized;
        
        // Detach from parent
        _trash.transform.SetParent(null);
        
        // Setup VR interactions
        SetupVRGrabbable(_trash);
        
        Rigidbody rb = _trash.GetComponent<Rigidbody>();
        
        // Apply force to throw the object
        rb.AddForce(direction * throwForce, ForceMode.Impulse);
        
        _data.state = "Idle";
    }

    private void Move(float speed)
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }
    
    private void SetupVRGrabbable(GameObject thrownObject)
    {
        XRGrabInteractable grabbable = thrownObject.GetComponent<XRGrabInteractable>();
        if (!grabbable)
        {
            grabbable = thrownObject.AddComponent<XRGrabInteractable>();
        }
        grabbable.enabled = true;  // Enable the interactable when thrown
        
        // Configure basic grab settings
        grabbable.movementType = XRBaseInteractable.MovementType.VelocityTracking;
        grabbable.throwOnDetach = true;
        
        // Make sure there's a Rigidbody
        Rigidbody rb = thrownObject.GetComponent<Rigidbody>();
        if (!rb)
        {
            rb = thrownObject.AddComponent<Rigidbody>();
        }
        rb.useGravity = true;
        rb.isKinematic = false;
        
        // Ensure there's a collider if not already present
        if (thrownObject.GetComponent<Collider>() == null)
        {
            thrownObject.AddComponent<BoxCollider>();
        }
    }
}