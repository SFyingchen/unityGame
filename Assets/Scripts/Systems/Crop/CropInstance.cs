using System;
using UnityEngine;

namespace Lingtianlu
{
    [Serializable]
    public class CropInstance
    {
        public CropDefinition Definition;
        public int PlantedOnDay;
        public float Progress;
        public bool IsHarvested;

        public CropInstance(CropDefinition definition, int plantedOnDay)
        {
            Definition = definition;
            PlantedOnDay = plantedOnDay;
            Progress = 0f;
            IsHarvested = false;
        }

        public bool IsMature => Progress >= 1f && !IsHarvested;

        public void Advance(float growthModifier)
        {
            if (IsHarvested || Definition == null)
            {
                return;
            }

            float dailyProgress = 1f / Math.Max(1, Definition.BaseGrowthDays);
            Progress = Mathf.Clamp(Progress + dailyProgress * growthModifier, 0f, 1f);
        }
    }
}
