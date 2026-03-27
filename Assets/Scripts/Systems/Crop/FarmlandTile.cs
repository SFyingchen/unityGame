using System;
using UnityEngine;

namespace Lingtianlu
{
    public enum FarmlandTileState
    {
        Untilled,
        Tilled,
        Planted,
        ReadyToHarvest
    }

    [Serializable]
    public class FarmlandTile
    {
        public Vector2Int Position;
        public FarmlandTileState State;
        public CropInstance Crop;
        public bool WateredToday;

        public FarmlandTile(Vector2Int position)
        {
            Position = position;
            State = FarmlandTileState.Untilled;
            Crop = null;
            WateredToday = false;
        }
    }
}
