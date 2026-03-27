using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lingtianlu
{
    public readonly struct HarvestEvent
    {
        public readonly CropDefinition Crop;
        public readonly int Amount;

        public HarvestEvent(CropDefinition crop, int amount)
        {
            Crop = crop;
            Amount = amount;
        }
    }

    public class CropSystem : MonoBehaviour
    {
        [SerializeField] private WeatherSystem weatherSystem;

        private readonly List<CropInstance> activeCrops = new();

        public event Action<HarvestEvent> Harvested;

        public IReadOnlyList<CropInstance> ActiveCrops => activeCrops;

        public void Plant(CropDefinition cropDefinition, int currentDay)
        {
            if (cropDefinition == null)
            {
                return;
            }

            activeCrops.Add(new CropInstance(cropDefinition, currentDay));
        }

        public void AdvanceAllCrops(int day)
        {
            float growthModifier = weatherSystem == null ? 1f : weatherSystem.GetGrowthModifier();
            foreach (CropInstance crop in activeCrops)
            {
                crop.Advance(growthModifier);
            }
        }

        public bool TryHarvest(CropInstance crop)
        {
            if (crop == null || !crop.IsMature)
            {
                return false;
            }

            crop.IsHarvested = true;
            Harvested?.Invoke(new HarvestEvent(crop.Definition, crop.Definition.YieldCount));
            return true;
        }
    }
}
