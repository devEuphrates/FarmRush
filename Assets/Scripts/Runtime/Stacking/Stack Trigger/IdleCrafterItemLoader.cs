using UnityEngine;

namespace Euphrates
{
    public class IdleCrafterItemLoader : ItemLoader
    {
        [Space]
        [SerializeField] Crafter _crafter;

        protected override void DoAction()
        {
            TryAdd();
        }

        void TryAdd()
        {
            if (_crafter.CurrentState == StationState.Idle)
                base.DoAction();
        }
    }
}
