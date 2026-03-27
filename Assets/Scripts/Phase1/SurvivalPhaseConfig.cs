using UnityEngine;

namespace Lingtianlu
{
    [CreateAssetMenu(menuName = "Lingtianlu/Phase1 Survival Config", fileName = "SurvivalPhaseConfig")]
    public class SurvivalPhaseConfig : ScriptableObject
    {
        [field: SerializeField, Min(1)] public int TargetHarvestCount { get; private set; } = 20;
        [field: SerializeField, Min(1)] public int TargetTilledTiles { get; private set; } = 12;
        [field: SerializeField, Min(1)] public int TargetGold { get; private set; } = 1000;
        [field: SerializeField, Min(1)] public int RecoveryPerDay { get; private set; } = 30;
    }
}
