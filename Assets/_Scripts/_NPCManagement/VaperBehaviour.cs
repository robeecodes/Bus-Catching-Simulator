using UnityEngine;

public class VaperBehaviour : MonoBehaviour, INPCBehavior
{
    [SerializeField] private GameObject vapePrefab;
    [SerializeField] private Transform vapeSpawnTarget;
    
    private static readonly int Vape = Animator.StringToHash("Vape");
    private NPCController _npcController;
    
    private enum VapeStates
    {
        Vape,
        Idle
    }
    
    private VapeStates _state;

    public void Init(NPCController npcController)
    {
        _npcController = npcController;
        _state = VapeStates.Idle;
    }

    public void Activate()
    {
        _state = VapeStates.Vape;
    }
    
    public void HandleState()
    {
        if (_state == VapeStates.Vape)
        {
            VapeAtPlayer();
        }
    }

    public void SpawnSmoke()
    {
        Instantiate(vapePrefab, vapeSpawnTarget.position, vapeSpawnTarget.rotation);
    }
    
    private void VapeAtPlayer()
    {
        _npcController.animator.SetTrigger(Vape);
        _state = VapeStates.Idle;
    }
}