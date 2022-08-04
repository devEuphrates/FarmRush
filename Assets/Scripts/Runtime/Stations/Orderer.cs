using System;
using System.Collections.Generic;
using UnityEngine;

namespace Euphrates
{
    public class Orderer : StackActionTrigger
    {
        bool _isOrderComplete = false;

        [SerializeField] PlayerUnlocksSO _playerUnlocks;

        [Header("Settings"), Space]
        [SerializeReference] IntSO MaxOrderCount;
        [SerializeReference] IntSO MaxOrderAmount;

        [Header("References"), Space]
        [SerializeReference] OrderDisplayer _orderDisplayer;

        List<ItemOrder> _orders = new List<ItemOrder>();

        public event Action OnOrderSatisfied;

        protected override void DoAction()
        {
            if (_isOrderComplete || _orders.Count <= 0)
                return;

            bool notSatisfied = false;

            foreach (var order in _orders)
            {
                if (order.IsSatisfied())
                    continue;

                notSatisfied = true;

                if (!_ownStack.CanAddItem() || !_stack.CanRemoveItem(order.OrderedItem))
                    continue;

                Item itm = _stack.RemoveItem(order.OrderedItem);
                itm.gameObject.SetLayer(0, true);
                _ownStack.AddItem(itm);
                order.AddAmount();
            }

            _orderDisplayer.UpdateOrders();

            if (!notSatisfied)
                OrderComplete();

            base.DoAction();
        }

        public void RandomOrder()
        {
            ClearOrders();

            int itmCount = Mathf.Clamp(_playerUnlocks.UnlockedItems.Count, 1, MaxOrderCount + 1);
            int orderCount = UnityEngine.Random.Range(1, itmCount + 1);
            List<ItemSO> orderItems = new List<ItemSO>(_playerUnlocks.UnlockedItems);

            for (int i = 0; i < orderCount; i++)
            {
                ItemSO itm = orderItems.GetRandomItem();
                ItemOrder order = new ItemOrder(itm, UnityEngine.Random.Range(1, MaxOrderAmount));
                orderItems.RemoveAll(p => p == itm);
                _orders.Add(order);
            }

            _orderDisplayer.ShowOrders(_orders);
        }

        public void ClearOrders()
        {
            _isOrderComplete = false;
            _orderDisplayer.HideOrders();
            _orders.Clear();
        }

        public void ClearItems()
        {
            int cnt = _ownStack.GetItemCount();
            for (int i = 0; i < cnt; i++)
            {
                Item itm = _ownStack.RemoveItem();
                SpawnManager.ReleaseItem(itm);
            }
        }

        void OrderComplete()
        {
            _isOrderComplete = true;
            OnOrderSatisfied?.Invoke();
        }

        public void ResetEvents()
        {
            OnOrderSatisfied = null;
        }

        public int GetOrdersWorth()
        {
            int sum = 0;

            foreach (var order in _orders)
                sum += order.OrderedItem.ItemWorth * order.ExpectedAmount;

            return sum;
        }
    }
}
