using UnityEngine;

namespace Euphrates
{
    [CreateAssetMenu(fileName = "New Unlockable Item", menuName = "Progression/Unlockable Item")]
	public class UnlockableItemSO : UnlockableSO
	{
		[SerializeField] ItemSO _unlockedItem;

        public override void Unlock()
        {
            _playerUnlocks.UnlockItem(_unlockedItem);
            base.Unlock();
        }
    }
}
