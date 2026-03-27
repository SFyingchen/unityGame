using UnityEngine;

namespace Lingtianlu
{
    /// <summary>
    /// Wires core runtime systems so they can communicate through events.
    /// Attach to an empty GameObject in the first scene.
    /// </summary>
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private TimeSystem timeSystem;
        [SerializeField] private WeatherSystem weatherSystem;
        [SerializeField] private CropSystem cropSystem;
        [SerializeField] private EconomySystem economySystem;
        [SerializeField] private NPCSystem npcSystem;
        [SerializeField] private AlchemySystem alchemySystem;
        [SerializeField] private InventorySystem inventorySystem;

        private void Awake()
        {
            if (timeSystem == null || weatherSystem == null || cropSystem == null || economySystem == null || npcSystem == null || alchemySystem == null)
            {
                Debug.LogWarning("GameBootstrap is missing one or more system references.");
                return;
            }

            timeSystem.DayChanged += weatherSystem.GenerateDailyWeather;
            timeSystem.DayChanged += cropSystem.AdvanceAllCrops;
            timeSystem.DayChanged += economySystem.RefreshDailyPrices;
            timeSystem.DayChanged += npcSystem.PlanDailySchedule;

            cropSystem.Harvested += economySystem.RegisterHarvestSupply;
            cropSystem.Harvested += npcSystem.ReactToHarvest;
            cropSystem.Harvested += OnHarvestAddInventory;
            alchemySystem.DanCreated += npcSystem.ReactToAlchemy;
        }

        private void OnDestroy()
        {
            if (timeSystem == null || weatherSystem == null || cropSystem == null || economySystem == null || npcSystem == null || alchemySystem == null)
            {
                return;
            }

            timeSystem.DayChanged -= weatherSystem.GenerateDailyWeather;
            timeSystem.DayChanged -= cropSystem.AdvanceAllCrops;
            timeSystem.DayChanged -= economySystem.RefreshDailyPrices;
            timeSystem.DayChanged -= npcSystem.PlanDailySchedule;

            cropSystem.Harvested -= economySystem.RegisterHarvestSupply;
            cropSystem.Harvested -= npcSystem.ReactToHarvest;
            cropSystem.Harvested -= OnHarvestAddInventory;
            alchemySystem.DanCreated -= npcSystem.ReactToAlchemy;
        }

        private void OnHarvestAddInventory(HarvestEvent harvest)
        {
            if (inventorySystem == null || harvest.Crop == null)
            {
                return;
            }

            inventorySystem.AddItem(harvest.Crop.CropId, harvest.Amount);
        }
    }
}
