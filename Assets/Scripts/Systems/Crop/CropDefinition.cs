using UnityEngine;

namespace Lingtianlu
{
    [CreateAssetMenu(menuName = "Lingtianlu/Crop Definition", fileName = "CropDefinition")]
    public class CropDefinition : ScriptableObject
    {
        [field: SerializeField] public string CropId { get; private set; }
        [field: SerializeField] public string DisplayName { get; private set; }
        [field: SerializeField, Min(1)] public int BaseGrowthDays { get; private set; } = 4;
        [field: SerializeField, Min(1)] public int YieldCount { get; private set; } = 1;
        [field: SerializeField, Min(1)] public int BaseMarketPrice { get; private set; } = 20;
        [field: SerializeField, Range(0f, 1f)] public float SpiritAffinity { get; private set; } = 0.5f;
    }
}
