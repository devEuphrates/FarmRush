using System.Collections.Generic;
using UnityEngine;

namespace Euphrates
{
    public class InfiniteStacker : Stacker
    {
        [SerializeField] bool _drawGizmo = false;

        List<Item> _items = new List<Item>();
        [Header("Cell Properties")]
        [SerializeField] Vector3 _cellSize = Vector3.one;
        [SerializeField] int _rowCount = 1, _columnCount = 1;

        [Header("Items Properties"), Space]
        [SerializeField] Transform _socket;
        [SerializeField] int _maxItemCount = 5;

        [SerializeField, Space] FloatSO _itemFloatDuration;

        public override bool CanAddItem() => CanTakeItems && _items.Count < _maxItemCount;

        public override bool CanRemoveItem(ItemSO itemInfo)
        {
            if (!CanGiveItems)
                return false;

            if (itemInfo == null)
                return _items.Count > 0;

            bool check = false;

            for (int i = _items.Count - 1; i >= 0; i--)
            {
                if (_items[i].ItemInfo == itemInfo)
                {
                    check = true;
                    break;
                }
            }

            return check;
        }

        public override void AddItem(Item item)
        {
            item.transform.parent = null;
            _items.Add(item);
            item.FloatToPos(_itemFloatDuration, GetPos(_items.Count - 1), Quaternion.identity, 0f, () => item.transform.parent = _socket, _socket);

            if (_playSounds)
                SoundManager.PlayClip("pick", 1);

            base.AddItem(null);
        }

        public override void AddItemInstant(Item item)
        {
            _items.Add(item);
            item.transform.parent = _socket;
            item.SnapToPos(GetPos(_items.Count - 1), Quaternion.identity);

            base.AddItemInstant(null);
        }

        public override Item RemoveItem(ItemSO itemInfo)
        {
            Item item = null;

            void Removal(int index)
            {
                _items[index].transform.parent = null;
                _items.RemoveAt(index);
                base.RemoveItem(null);
            }

            if (itemInfo == null)
            {
                item = _items[_items.Count - 1];
                Removal(_items.Count - 1);
                return item;
            }

            int indx = 0;

            for (int i = _items.Count - 1; i >= 0; i--)
            {
                if (_items[i].ItemInfo == itemInfo)
                {
                    item = _items[i];
                    indx = i;
                    break;
                }
                _items[i].transform.parent = _socket;
                _items[i].FloatToPos(0.15f, GetPos(i - 1), Quaternion.identity, 0f,
                    null, _socket);
            }

            if (item == null)
                return null;

            //item.transform.parent = null;
            //_items.RemoveAt(indx);
            //_itemCount--;

            //base.RemoveItem(null);

            Removal(indx);

            return item;
        }

        public Vector3 GetPos(int index)
        {
            int y = index / (_rowCount * _columnCount);
            index %= _rowCount * _columnCount;

            int x = index / _rowCount;
            int z = index % _rowCount;

            return new Vector3((float)x * _cellSize.x, (float)y * _cellSize.y, (float)z * _cellSize.z);
        }

        public override int GetItemCount(ItemSO item = null)
        {
            if (item == null)
                return _items.Count;

            int count = 0;
            foreach (var itm in _items)
                if (itm.ItemInfo == item)
                    count++;

            return count;
        }

        public override int GetSize()
        {
            return _maxItemCount;
        }

        void OnDrawGizmos()
        {
            if (!_drawGizmo)
                return;

            Gizmos.matrix = _socket.localToWorldMatrix;
            Gizmos.color = Color.yellow;

            for (int i = 0; i < _maxItemCount; i++)
            {
                Vector3 pos = GetPos(i);
                Gizmos.DrawWireCube(pos, _cellSize);
            }
        }
    }
}
