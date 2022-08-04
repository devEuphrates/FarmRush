using UnityEngine;

namespace Euphrates
{
    [System.Serializable]
    public class ItemOrder
    {
        public ItemSO OrderedItem;
        readonly int _amount;
        int _satisfiedAmount;
        
        public int ExpectedAmount { get { return _amount; } }
        public int SatisfiedAmount { get { return _satisfiedAmount; } }

        public ItemOrder(ItemSO item, int amount)
        {
            OrderedItem = item;
            _amount = amount;
            _satisfiedAmount = 0;
        }

        public bool IsSatisfied() => _satisfiedAmount >= _amount;

        public void AddAmount(int amount = 1) => _satisfiedAmount = Mathf.Clamp(_satisfiedAmount + amount, 0, _amount);

    }
}
