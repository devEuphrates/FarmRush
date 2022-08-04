using System.Collections.Generic;
using UnityEngine;

namespace Euphrates
{
    public class FiniteStacker : Stacker
    {
        [Header("Cell Properties")]
        [SerializeField] List<Socket> _sockets = new List<Socket>();

        [Header("Items Properties"), Space]
        [SerializeField] int _maxItemCount = 5;

        [SerializeField, Space] FloatSO _itemFloatDuration;

        int _itemCount = 0;

        public int ItemCount { get { return _itemCount; } }
        public int MaxItemCount { get { return _maxItemCount; } }

        public override bool CanAddItem() => CanTakeItems && _itemCount < _maxItemCount && _itemCount < _sockets.Count;

        public override bool CanRemoveItem(ItemSO itemInfo)
        {
            if (!CanGiveItems)
                return false;

            if (itemInfo == null)
                return _itemCount > 0;

            bool check = false;
            for (int i = _itemCount - 1; i >= 0; i--)
            {
                if (_sockets[i].StoredItem.ItemInfo == itemInfo)
                {
                    check = true;
                    break;
                }
            }

            return check;
        }

        public override void AddItem(Item item)
        {
            Socket selSocket = _sockets[_itemCount++];
            selSocket.StoredItem = item;
            item.FloatToPos(_itemFloatDuration, Vector3.zero, Quaternion.identity, 0f, () => item.transform.parent = selSocket.SocketTransform, selSocket.SocketTransform);

            if (_playSounds)
                SoundManager.PlayClip("pick", 1);

            base.AddItem(null);
        }

        public override void AddItemInstant(Item item)
        {
            Socket selSocket = _sockets[_itemCount++];
            selSocket.StoredItem = item;
            item.transform.parent = selSocket.SocketTransform;
            item.SnapToPos(Vector3.zero, Quaternion.identity);

            base.AddItemInstant(null);
        }

        public override Item RemoveItem(ItemSO itemInfo)
        {
            if (itemInfo == null)
            {
                Item itm = _sockets[_itemCount - 1].StoredItem;

                itm.transform.parent = null;
                _sockets[--_itemCount].StoredItem = null;

                base.RemoveItem(null);
                return itm;
            }

            Item item = null;
            int indx = 0;

            for (int i = _itemCount - 1; i >= 0; i--)
            {
                if (_sockets[i].StoredItem.ItemInfo == itemInfo)
                {
                    item = _sockets[i].StoredItem;
                    indx = i;
                    break;
                }
            }

            if (item == null)
                return null;

            item.transform.parent = null;
            _sockets[indx].StoredItem = null;
            for (int i = indx + 1; i < _itemCount; i++)
            {
                _sockets[i - 1].StoredItem = _sockets[i].StoredItem;
                _sockets[i - 1].StoredItem.transform.parent = _sockets[i - 1].SocketTransform;
                _sockets[i - 1].StoredItem.SnapToPos(Vector3.zero, Quaternion.identity);
            }

            _sockets[--_itemCount].StoredItem = null;

            base.RemoveItem(null);
            return item;
        }

        public override int GetItemCount(ItemSO item = null)
        {
            if (item == null)
                return _itemCount;

            int count = 0;
            foreach (var sckt in _sockets)
                if (sckt.StoredItem != null && sckt.StoredItem.ItemInfo == item)
                    count++;

            return count;
        }

        public override int GetSize()
        {
            return MaxItemCount;
        }

        // Try methodes most likely won't be used
        public bool TryAddItem(Item item)
        {
            if (_itemCount >= _maxItemCount || _itemCount >= _sockets.Count)
                return false;

            Socket selSocket = _sockets[_itemCount++];
            selSocket.StoredItem = item;
            item.FloatToPos(_itemFloatDuration, Vector3.zero, Quaternion.identity, 0f, () => item.transform.parent = selSocket.SocketTransform, selSocket.SocketTransform);

            return true;
        }

        public bool TryRemoveItem(out Item item, ItemSO itemInfo)
        {
            item = null;
            int indx = 0;

            for (int i = _itemCount - 1; i >= 0; i--)
            {
                if (_sockets[i].StoredItem.ItemInfo == itemInfo)
                {
                    item = _sockets[i].StoredItem;
                    indx = i;
                    break;
                }
            }

            if (item == null)
                return false;

            item.transform.parent = null;
            for (int i = indx + 1; i < _itemCount; i++)
            {
                _sockets[i - 1].StoredItem = _sockets[i].StoredItem;
                _sockets[i - 1].StoredItem.transform.parent = _sockets[i - 1].SocketTransform;
                _sockets[i - 1].StoredItem.SnapToPos(Vector3.zero, Quaternion.identity);
            }

            _sockets[--_itemCount].StoredItem = null;

            return true;
        }
    }

    [System.Serializable]
    public class Socket
    {
        public Transform SocketTransform;
        public Item StoredItem;

        public Socket(Transform transform = null, Item item = null)
        {
            SocketTransform = transform;
            StoredItem = item;
        }
    }
}
