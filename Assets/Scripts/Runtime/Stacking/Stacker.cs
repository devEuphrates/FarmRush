using System;
using UnityEngine;

namespace Euphrates
{
    public class Stacker : MonoBehaviour
    {
        public event Action onItemAdded;
        public event Action onItemRemoved;

        public bool CanTakeItems = true;
        public bool CanGiveItems = true;

        [SerializeField] protected bool _playSounds = false;

        public virtual bool CanAddItem()
        {
            print("Stacker.CanAddItem not implemented");
            return false;
        }

        public virtual bool CanRemoveItem(ItemSO itemInfo = null)
        {
            print("Stacker.CanRemoveItem not implemented");
            return false;
        }

        public virtual void AddItem(Item item) => onItemAdded?.Invoke();

        public virtual void AddItemInstant(Item item) => onItemAdded?.Invoke();

        public virtual Item RemoveItem(ItemSO itemInfo = null)
        {
            onItemRemoved?.Invoke();
            return null;
        }

        public virtual int GetItemCount(ItemSO item = null) => 0;

        public virtual int GetSize() => 0;
    }
}
