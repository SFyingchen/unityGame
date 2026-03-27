using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lingtianlu
{
    public class EconomySystem : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float marketVolatility = 0.18f;

        private readonly Dictionary<string, int> priceTable = new();
        private readonly Dictionary<string, int> dailySupply = new();

        public event Action<int> GoldEarned;

        public int GetPrice(CropDefinition crop)
        {
            if (crop == null)
            {
                return 0;
            }

            if (!priceTable.TryGetValue(crop.CropId, out int price))
            {
                price = crop.BaseMarketPrice;
                priceTable[crop.CropId] = price;
            }

            return price;
        }

        public int SellCrop(CropDefinition crop, int amount)
        {
            if (crop == null || amount <= 0)
            {
                return 0;
            }

            int revenue = GetPrice(crop) * amount;
            GoldEarned?.Invoke(revenue);
            RegisterHarvestSupply(new HarvestEvent(crop, amount));
            return revenue;
        }

        public void RegisterHarvestSupply(HarvestEvent harvest)
        {
            if (harvest.Crop == null)
            {
                return;
            }

            if (!dailySupply.ContainsKey(harvest.Crop.CropId))
            {
                dailySupply[harvest.Crop.CropId] = 0;
            }

            dailySupply[harvest.Crop.CropId] += harvest.Amount;
        }

        public void RefreshDailyPrices(int day)
        {
            List<string> keys = new(priceTable.Keys);
            foreach (string cropId in keys)
            {
                int supply = dailySupply.TryGetValue(cropId, out int count) ? count : 0;
                float supplyPenalty = Mathf.Clamp01(supply / 40f) * 0.2f;
                float randomSwing = Random.Range(-marketVolatility, marketVolatility);
                float multiplier = 1f + randomSwing - supplyPenalty;
                int nextPrice = Mathf.Max(1, Mathf.RoundToInt(priceTable[cropId] * multiplier));
                priceTable[cropId] = nextPrice;
            }

            dailySupply.Clear();
        }
    }
}
