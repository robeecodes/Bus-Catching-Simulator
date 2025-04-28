using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class ThrowerBehaviour : MonoBehaviour, INPCBehavior
{
    private static readonly int Throw = Animator.StringToHash("Throw");
    private NPCController _npcController;

    private GameObject _trash;
    [SerializeField] private Transform trashSpawnTarget;

    [SerializeField] private float throwForce = 10f;
    [SerializeField] private GameObject[] trashObjects;

    private Rigidbody _rb;
    private XRGrabInteractable _grabbable;
    
    private enum TrashStates
    {
        Throw,
        Idle
    }

    private TrashStates _state;

    public void Init(NPCController npcController)
    {
        _npcController = npcController;
        
        _state = TrashStates.Idle;
    }

    public void HandleState()
    {
        if (_state == TrashStates.Throw)
        {
            ThrowTrash();
        }
    }

    public void Activate()
    {
        SpawnTrash();
        _state = TrashStates.Throw;
    }

    private void SpawnTrash()
    {
        var selectedTrashObject = trashObjects[Random.Range(0, trashObjects.Length)];
        _trash = Instantiate(selectedTrashObject, trashSpawnTarget.position, trashSpawnTarget.rotation);

        _rb = _trash.GetComponent<Rigidbody>();
        _rb.isKinematic = true;

        _grabbable = _trash.GetComponent<XRGrabInteractable>();
        _grabbable.enabled = false;
    }

    private void ThrowTrash()
    {
        Vector3 throwTargetPosition = _npcController.playerTransform.position;
        throwTargetPosition.y += 0.2f;

        _npcController.transform.LookAt(throwTargetPosition);
        _npcController.animator.SetTrigger(Throw);

        Vector3 direction = (throwTargetPosition - _trash.transform.position).normalized;

        _trash.transform.SetParent(null);

        SetupVRGrabbable();
        _rb.WakeUp();
        _rb.AddForce(direction * throwForce, ForceMode.Impulse);

        StartCoroutine(ResetNPCPose());

        _state = TrashStates.Idle;
    }

    private IEnumerator ResetNPCPose()
    {
        yield return new WaitForSeconds(1.0f);

        float elapsed = 0f;
        float duration = 0.5f;

        Quaternion startRotation = _npcController.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0f, startRotation.eulerAngles.y, 0f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _npcController.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / duration);
            yield return null;
        }
        
        _trash.GetComponent<Trash>().wasThrown = true;

        _npcController.transform.rotation = targetRotation;
    }

    private void SetupVRGrabbable()
    {
        _grabbable.enabled = true;

        _grabbable.movementType = XRBaseInteractable.MovementType.VelocityTracking;
        _grabbable.throwOnDetach = true;

        _rb.useGravity = true;
        _rb.isKinematic = false;
    }
}