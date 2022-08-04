using UnityEngine;

namespace Euphrates
{
    public class UnlockableObject : MonoBehaviour
    {
        [SerializeField] UnlockableSO _unlockable;
        [SerializeField] PlayerUnlocksSO _unlocks;

        private void Start()
        {
            if (!_unlocks.UnlockedItems.Exists(p => p == _unlockable))
                return;

            gameObject.SetActive(true);
        }

        void OnEnable()
        {
            _unlockable.OnUnlock += Unlocked;
        }

        void OnDisable()
        {
            _unlockable.OnUnlock -= Unlocked;
        }

        void Unlocked()
        {
            gameObject.SetActive(true);
        }
    }
}
