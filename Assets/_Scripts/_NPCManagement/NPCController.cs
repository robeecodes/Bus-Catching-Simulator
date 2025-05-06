using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NPCController : MonoBehaviour
{
    public GameObject headBone;

    // Player positions
    public Transform playerTransform;
    [SerializeField] private Transform playerEyeSpawn;
    
    public Animator animator;
    
    [SerializeField] private float cooldown = 10f;

    private INPCBehavior _behaviour;

    private bool _canTrigger = true;
    
    private void Awake()
    {
        _behaviour = GetComponent<INPCBehavior>();
        _behaviour?.Init(this);
        TryGetComponent<Animator>(out animator);
    }

    private void Update()
    {
        _behaviour?.HandleState();
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.lightingManager.TimeOfDay >= 21 || GameManager.Instance.lightingManager.TimeOfDay <= 3)
        {
            headBone.transform.LookAt(playerTransform);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<Trash>(out Trash trash))
        {
            if (trash.wasThrown)
            {
                XRGrabInteractable grabInteractable = other.gameObject.GetComponent<XRGrabInteractable>();

                if (grabInteractable != null && grabInteractable.isSelected)
                {
                    grabInteractable.interactionManager.SelectExit(grabInteractable.firstInteractorSelecting,
                        grabInteractable);
                }

                ReturnTrashToPlayer(other.gameObject);
            }
        }
    }

    private void ReturnTrashToPlayer(GameObject trash)
    {
        Rigidbody rb = trash.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 direction = (new Vector3(playerEyeSpawn.position.x, playerEyeSpawn.position.y + 0.2f, playerEyeSpawn.position.z) - trash.transform.position).normalized;
            float returnForce = 10f;

            rb.AddForce(direction * returnForce, ForceMode.Impulse);
        }
    }


    public void Activate()
    {
        if (!_canTrigger) return;
        _behaviour?.Activate();
        StartCoroutine(TriggerCooldown());
    }

    private IEnumerator TriggerCooldown()
    {
        _canTrigger = false;
        yield return new WaitForSeconds(cooldown);
        _canTrigger = true;
    }
}