using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New NPC State", menuName = "NPC State")]
public class NPCData : ScriptableObject
{
    public NPCType type;
    public string state;
    public bool triggered;
}