using System;
using System.Collections.Generic;
using UnityEngine;

namespace Euphrates
{
	[CreateAssetMenu(fileName = "New Player Unlocks Holder", menuName = "Progression/Unlocks Holder")]
	public class PlayerUnlocksSO : ScriptableObject
	{
		public List<ItemSO> UnlockedItems;
		public event Action OnItemUnlocked;

		[SerializeField] Dictionary<UnlockableSO, bool> _unlockables = new Dictionary<UnlockableSO, bool>();

        public void UnlockUnlockable(UnlockableSO unlockable) => _unlockables[unlockable] = true;

        public void UnlockItem(ItemSO item)
        {
			UnlockedItems.Add(item);
			OnItemUnlocked?.Invoke();
        }
	}
}
