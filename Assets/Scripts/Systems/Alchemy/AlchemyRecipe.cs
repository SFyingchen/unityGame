using System.Collections.Generic;
using UnityEngine;

namespace Lingtianlu
{
    [CreateAssetMenu(menuName = "Lingtianlu/Alchemy Recipe", fileName = "AlchemyRecipe")]
    public class AlchemyRecipe : ScriptableObject
    {
        [field: SerializeField] public string RecipeId { get; private set; }
        [field: SerializeField] public string OutputDanName { get; private set; }
        [field: SerializeField, Min(1)] public int BaseQuality { get; private set; } = 50;
        [field: SerializeField] public List<CropDefinition> Ingredients { get; private set; } = new();
    }
}
