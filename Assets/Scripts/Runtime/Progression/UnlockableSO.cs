using System;
using System.Collections.Generic;
using UnityEngine;

namespace Euphrates
{
    [CreateAssetMenu(fileName = "New Unlockable", menuName = "Progression/Unlockable")]
    public class UnlockableSO : ScriptableObject
    {
        [SerializeField] protected PlayerUnlocksSO _playerUnlocks;
        [SerializeField] List<UnlockableSO> _requirements = new List<UnlockableSO>();

        [SerializeField] bool _isUnlocked = false;
        public bool IsUnlocked { get { return _isUnlocked; } }

        public event Action OnUnlock;

        void OnEnable()
        {
            CheckRequirements();

            foreach (var req in _requirements)
                req.OnUnlock += CheckRequirements;
        }

        void OnDisable()
        {
            foreach (var req in _requirements)
                req.OnUnlock -= CheckRequirements;
        }

        void CheckRequirements()
        {
            foreach (var req in _requirements)
                if (!req._isUnlocked)
                    return;

            Unlock();
        }

        public virtual void Unlock()
        {
            _isUnlocked = true;
            _playerUnlocks.UnlockUnlockable(this);
            OnUnlock?.Invoke();
        }
    }
}
