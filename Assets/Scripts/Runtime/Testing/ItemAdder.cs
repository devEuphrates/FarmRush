using System.Collections.Generic;
using UnityEngine;

namespace Euphrates
{
    [RequireComponent(typeof(Stacker))]
    public class ItemAdder : MonoBehaviour
    {
        [SerializeField] Stacker _stack;
        [HideInInspector] public List<ItemAdderSpawnInfo> Items;

        public void SpawnItems()
        {
            if (_stack == null)
                return;

            foreach (var itmInf in Items)
            {
                for (int i = 0; i < itmInf.AddedAmount; i++)
                {
                    Item itm = Instantiate(itmInf.AddedItem.ItemPrefab).GetComponent<Item>();
                    _stack.AddItemInstant(itm);
                }
            }
        }
    }

    [System.Serializable]
    public class ItemAdderSpawnInfo
    {
        public ItemSO AddedItem;
        public int AddedAmount;
    }
}
