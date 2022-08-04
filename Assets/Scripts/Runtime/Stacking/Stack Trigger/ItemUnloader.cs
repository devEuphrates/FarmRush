using System.Collections.Generic;
using UnityEngine;

namespace Euphrates
{
    public class ItemUnloader : StackActionTrigger
    {
        [Space]
        [SerializeField] bool _allItems = false;
        [SerializeField] List<ItemSO> _itemsInfo;

        protected override void DoAction()
        {
            if (_allItems)
            {
                if (!_ownStack.CanAddItem() || !_stack.CanRemoveItem())
                    return;

                Item item = _stack.RemoveItem();
                _ownStack.AddItem(item);
                base.DoAction();

                return;
            }

            foreach (var itm in _itemsInfo)
            {
                if (!_stack.CanAddItem())
                    return;

                if (!_ownStack.CanRemoveItem(itm))
                    continue;

                Item item = _ownStack.RemoveItem(itm);
                _stack.AddItem(item);
                base.DoAction();
            }
        }

        protected override void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            base.OnDrawGizmos();
        }
    }
}
