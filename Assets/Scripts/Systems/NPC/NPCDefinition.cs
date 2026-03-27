using UnityEngine;

namespace Lingtianlu
{
    public enum PersonalityType
    {
        Kind,
        Greedy,
        Cunning
    }

    [CreateAssetMenu(menuName = "Lingtianlu/NPC Definition", fileName = "NPCDefinition")]
    public class NPCDefinition : ScriptableObject
    {
        [field: SerializeField] public string NpcId { get; private set; }
        [field: SerializeField] public string DisplayName { get; private set; }
        [field: SerializeField] public PersonalityType Personality { get; private set; }
        [field: SerializeField, Range(-100, 100)] public int InitialFavorability { get; private set; }
    }
}
