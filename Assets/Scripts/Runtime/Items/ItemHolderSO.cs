using System.Collections.Generic;
using UnityEngine;

namespace Euphrates
{
	[CreateAssetMenu(menuName = "Items And Crafting/Item Holder", order = 0)]
	public class ItemHolderSO : ScriptableObject
	{
		public List<ItemSO> Items = new List<ItemSO>();

		public ItemSO GetItem(int id)
        {
            foreach (var item in Items)
            {
				if (item.ItemID == id)
					return item;
            }

			return null;
        }
	}
}
