public interface INPCBehavior
{
    void HandleState();
    void Init(NPCController npcController);
    void Activate();
}