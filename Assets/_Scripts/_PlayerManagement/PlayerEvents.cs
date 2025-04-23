using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    [SerializeField] private Transform playerHeadTransform;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<NPCController>(out NPCController npcController))
        {
            var target = transform;
            var newPosition = target.position;
            newPosition.y = target.position.y - playerHeadTransform.position.y;
            target.position = newPosition;
            npcController.Activate(transform);
        }

    }
}
