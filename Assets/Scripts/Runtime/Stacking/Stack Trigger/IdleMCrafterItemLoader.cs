using UnityEngine;

namespace Euphrates
{
    public class IdleMCrafterItemLoader : ItemLoader
	{
        [Space]
        [SerializeField] MultiCrafter _crafter;

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
