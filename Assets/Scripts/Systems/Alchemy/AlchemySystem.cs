using System;
using UnityEngine;

namespace Lingtianlu
{
    public class AlchemySystem : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float luckFactor = 0.15f;

        public event Action<int> DanCreated;

        public int CraftDan(AlchemyRecipe recipe, float fireControl, float spiritResonance)
        {
            if (recipe == null)
            {
                return 0;
            }

            float fireBonus = Mathf.Clamp01(fireControl) * 20f;
            float resonanceBonus = Mathf.Clamp01(spiritResonance) * 20f;
            float randomBonus = UnityEngine.Random.Range(-15f, 15f) * (1f + luckFactor);

            int quality = Mathf.Clamp(Mathf.RoundToInt(recipe.BaseQuality + fireBonus + resonanceBonus + randomBonus), 1, 100);
            DanCreated?.Invoke(quality);
            return quality;
        }
    }
}
