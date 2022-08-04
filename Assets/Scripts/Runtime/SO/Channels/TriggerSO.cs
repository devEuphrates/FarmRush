using System;
using UnityEngine;

namespace Euphrates
{
    [CreateAssetMenu(fileName = "New Trigger", menuName = "SO Channels/Trigger")]
    public class TriggerSO : ScriptableObject
    {
        public event Action OnTrigger;

        public void Invoke() => OnTrigger?.Invoke();
    }
}