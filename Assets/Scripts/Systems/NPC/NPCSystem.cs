using System.Collections.Generic;
using UnityEngine;

namespace Lingtianlu
{
    public class NPCSystem : MonoBehaviour
    {
        [SerializeField] private List<NPCDefinition> npcDefinitions = new();

        private readonly Dictionary<string, int> favorability = new();

        private void Start()
        {
            foreach (NPCDefinition npc in npcDefinitions)
            {
                if (npc == null || favorability.ContainsKey(npc.NpcId))
                {
                    continue;
                }

                favorability[npc.NpcId] = npc.InitialFavorability;
            }
        }

        public void PlanDailySchedule(int day)
        {
            foreach (NPCDefinition npc in npcDefinitions)
            {
                if (npc == null)
                {
                    continue;
                }

                if (npc.Personality == PersonalityType.Kind && day % 7 == 0)
                {
                    ModifyFavorability(npc.NpcId, 1);
                }
            }
        }

        public void ReactToHarvest(HarvestEvent harvest)
        {
            foreach (NPCDefinition npc in npcDefinitions)
            {
                if (npc == null)
                {
                    continue;
                }

                int delta = npc.Personality switch
                {
                    PersonalityType.Kind => 1,
                    PersonalityType.Greedy => harvest.Amount > 2 ? 1 : 0,
                    PersonalityType.Cunning => 0,
                    _ => 0
                };

                ModifyFavorability(npc.NpcId, delta);
            }
        }

        public void ReactToAlchemy(int quality)
        {
            foreach (NPCDefinition npc in npcDefinitions)
            {
                if (npc == null)
                {
                    continue;
                }

                int delta = quality >= 80
                    ? 2
                    : quality >= 50
                        ? 1
                        : -1;

                ModifyFavorability(npc.NpcId, delta);
            }
        }

        public int GetFavorability(string npcId)
        {
            return favorability.TryGetValue(npcId, out int score) ? score : 0;
        }

        private void ModifyFavorability(string npcId, int delta)
        {
            if (!favorability.ContainsKey(npcId))
            {
                favorability[npcId] = 0;
            }

            favorability[npcId] = Mathf.Clamp(favorability[npcId] + delta, -100, 100);
        }
    }
}
