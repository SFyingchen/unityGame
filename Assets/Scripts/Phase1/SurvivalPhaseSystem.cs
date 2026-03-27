using System;
using UnityEngine;

namespace Lingtianlu
{
    /// <summary>
    /// 第一阶段（生存）目标：开垦农田、稳定收获、获得第一桶金。
    /// </summary>
    public class SurvivalPhaseSystem : MonoBehaviour
    {
        [SerializeField] private SurvivalPhaseConfig config;
        [SerializeField] private CropSystem cropSystem;
        [SerializeField] private EconomySystem economySystem;
        [SerializeField] private TimeSystem timeSystem;
        [SerializeField] private PlayerStatusSystem playerStatusSystem;

        public event Action PhaseCompleted;

        public int HarvestProgress { get; private set; }
        public int TilledTileProgress { get; private set; }
        public int GoldProgress { get; private set; }
        public bool IsCompleted { get; private set; }

        private void Awake()
        {
            if (cropSystem != null)
            {
                cropSystem.Harvested += OnHarvest;
                cropSystem.TileTilled += OnTileTilled;
            }

            if (economySystem != null)
            {
                economySystem.GoldEarned += OnGoldEarned;
            }

            if (timeSystem != null)
            {
                timeSystem.DayChanged += OnDayChanged;
            }
        }

        private void OnDestroy()
        {
            if (cropSystem != null)
            {
                cropSystem.Harvested -= OnHarvest;
                cropSystem.TileTilled -= OnTileTilled;
            }

            if (economySystem != null)
            {
                economySystem.GoldEarned -= OnGoldEarned;
            }

            if (timeSystem != null)
            {
                timeSystem.DayChanged -= OnDayChanged;
            }
        }

        private void OnHarvest(HarvestEvent harvest)
        {
            HarvestProgress += harvest.Amount;
            TryComplete();
        }

        private void OnTileTilled(TileEvent tileEvent)
        {
            TilledTileProgress += 1;
            TryComplete();
        }

        private void OnGoldEarned(int revenue)
        {
            GoldProgress += revenue;
            playerStatusSystem?.AddGold(revenue);
            TryComplete();
        }

        private void OnDayChanged(int day)
        {
            if (config == null)
            {
                return;
            }

            playerStatusSystem?.RecoverDaily(config.RecoveryPerDay);
        }

        private void TryComplete()
        {
            if (config == null || IsCompleted)
            {
                return;
            }

            bool done = HarvestProgress >= config.TargetHarvestCount
                        && TilledTileProgress >= config.TargetTilledTiles
                        && GoldProgress >= config.TargetGold;

            if (!done)
            {
                return;
            }

            IsCompleted = true;
            PhaseCompleted?.Invoke();
        }
    }
}
