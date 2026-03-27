using System;
using System.Collections.Generic;

namespace Lingtianlu
{
    [Serializable]
    public class SaveData
    {
        public int Day;
        public int Season;
        public int PlayerGold;
        public List<CropSaveItem> Crops = new();
        public List<NPCFavorabilitySaveItem> NpcRelations = new();
    }

    [Serializable]
    public class CropSaveItem
    {
        public string CropId;
        public float Progress;
        public bool IsHarvested;
    }

    [Serializable]
    public class NPCFavorabilitySaveItem
    {
        public string NpcId;
        public int Favorability;
    }
}
