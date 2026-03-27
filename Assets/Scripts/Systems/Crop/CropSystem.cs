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

    public readonly struct TileEvent
    {
        public readonly Vector2Int Position;

        public TileEvent(Vector2Int position)
        {
            Position = position;
        }
    }

    public class CropSystem : MonoBehaviour
    {
        [SerializeField] private WeatherSystem weatherSystem;
        [SerializeField, Min(0.1f)] private float dryGrowthPenalty = 0.45f;

        private readonly Dictionary<Vector2Int, FarmlandTile> tileMap = new();

        public event Action<HarvestEvent> Harvested;
        public event Action<TileEvent> TileTilled;

        public IEnumerable<FarmlandTile> ActiveTiles => tileMap.Values;

        public bool TryTill(Vector2Int position)
        {
            FarmlandTile tile = GetOrCreateTile(position);
            if (tile.State != FarmlandTileState.Untilled)
            {
                return false;
            }

            tile.State = FarmlandTileState.Tilled;
            TileTilled?.Invoke(new TileEvent(position));
            return true;
        }

        public bool TryPlant(Vector2Int position, CropDefinition cropDefinition, int currentDay)
        {
            if (cropDefinition == null)
            {
                return false;
            }

            FarmlandTile tile = GetOrCreateTile(position);
            if (tile.State != FarmlandTileState.Tilled)
            {
                return false;
            }

            tile.Crop = new CropInstance(cropDefinition, currentDay);
            tile.State = FarmlandTileState.Planted;
            return true;
        }

        public bool TryWater(Vector2Int position)
        {
            if (!tileMap.TryGetValue(position, out FarmlandTile tile) || tile.State == FarmlandTileState.Untilled)
            {
                return false;
            }

            tile.WateredToday = true;
            return true;
        }

        public void AdvanceAllCrops(int day)
        {
            float weatherModifier = weatherSystem == null ? 1f : weatherSystem.GetGrowthModifier();

            foreach (FarmlandTile tile in tileMap.Values)
            {
                if (tile.Crop == null || tile.State != FarmlandTileState.Planted)
                {
                    tile.WateredToday = false;
                    continue;
                }

                float waterModifier = tile.WateredToday ? 1f : dryGrowthPenalty;
                tile.Crop.Advance(weatherModifier * waterModifier);

                if (tile.Crop.IsMature)
                {
                    tile.State = FarmlandTileState.ReadyToHarvest;
                }

                tile.WateredToday = false;
            }
        }

        public bool TryHarvest(Vector2Int position)
        {
            if (!tileMap.TryGetValue(position, out FarmlandTile tile) || tile.State != FarmlandTileState.ReadyToHarvest || tile.Crop == null)
            {
                return false;
            }

            tile.Crop.IsHarvested = true;
            Harvested?.Invoke(new HarvestEvent(tile.Crop.Definition, tile.Crop.Definition.YieldCount));

            tile.Crop = null;
            tile.State = FarmlandTileState.Tilled;
            return true;
        }

        private FarmlandTile GetOrCreateTile(Vector2Int position)
        {
            if (!tileMap.TryGetValue(position, out FarmlandTile tile))
            {
                tile = new FarmlandTile(position);
                tileMap[position] = tile;
            }

            return tile;
        }
    }
}
