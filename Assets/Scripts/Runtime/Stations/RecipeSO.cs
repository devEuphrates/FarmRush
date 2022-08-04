using System.Collections.Generic;
using UnityEngine;

namespace Euphrates
{
    [CreateAssetMenu(fileName = "New Recipe", menuName = "Items And Crafting/Recipe")]
    public class RecipeSO : ScriptableObject
    {
        public List<RecipeNeed> Needs;
        public ItemSO Product;
        public int ProductCount = 1;
        public float Duration;
    }

    [System.Serializable]
    public struct RecipeNeed
    {
        public ItemSO NeededItem;
        public int NeededAmount;
    }
}
