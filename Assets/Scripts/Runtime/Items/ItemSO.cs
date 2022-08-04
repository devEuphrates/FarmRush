using UnityEngine;

namespace Euphrates
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Items And Crafting/Item")]
    public class ItemSO : ScriptableObject
    {
        public int ItemID;
        public string ItemName;
        public Sprite ItemIcon;
        public GameObject ItemPrefab;
        public int ItemWorth = 0;
    }
}
