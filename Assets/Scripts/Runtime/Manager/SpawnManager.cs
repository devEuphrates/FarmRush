using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Euphrates
{
    public class SpawnManager : Singleton<SpawnManager>
    {
        [SerializeField] int _initialPoolSize = 250;
        [SerializeField] int _maxPoolCount = 1000;

        [SerializeField] ItemHolderSO _items;

        readonly Dictionary<int, Pool> _itemPools = new Dictionary<int, Pool>();

        public async Task Init()
        {
            List<int> _initializingPools = new List<int>();
            foreach (var item in _items.Items)
            {
                Pool pool = new Pool(item.ItemPrefab, transform, _initialPoolSize, _maxPoolCount);
                _itemPools.Add(item.ItemID, pool);

                _initializingPools.Add(item.ItemID);
                pool.OnReady += () =>_initializingPools.Remove(item.ItemID);
            }

            while (_initializingPools.Count > 0)
                await Task.Yield();
        }

        public static Item SpawnItem(int itemID) => Instance._itemPools[itemID].Get().GetComponent<Item>();

        public static Item SpawnItem(int itemID, Transform parent) => Instance._itemPools[itemID].Get(parent).GetComponent<Item>();

        public static Item SpawnItem(int itemID, Transform parent, Vector3 position) => Instance._itemPools[itemID].Get(parent, position).GetComponent<Item>();

        public static Item SpawnItem(int itemID, Transform parent, Vector3 position, Quaternion rotation) => Instance._itemPools[itemID].Get(parent, position, rotation).GetComponent<Item>();

        public static void ReleaseItem(Item item)
        {
            if (!Instance._itemPools.ContainsKey(item.ItemInfo.ItemID))
            {
                Destroy(item.gameObject);
                return;
            }

            Pool pool = Instance._itemPools[item.ItemInfo.ItemID];

            if (!pool.TryRelease(item.gameObject))
                Destroy(item.gameObject);

            item.transform.parent = Instance.transform;
        }
    }

    class Pool
    {
        bool _isSet;
        public bool IsSet => _isSet;
        public event Action OnReady;

        readonly Transform _parent;
        readonly GameObject _pooledPrefab;
        readonly List<GameObject> _pooledObjects;
        int _currentIndex;

        readonly int _minCount;
        readonly int _maxCount;

        public Pool(GameObject prefab, Transform parent, int minCount, int maxCount)
        {
            _pooledPrefab = prefab;
            _parent = parent;

            _minCount = minCount;
            _maxCount = maxCount;

            _pooledObjects = new List<GameObject>();
            _isSet = false;

            SpawnObjectsAsync();

            _currentIndex = 0;
        }

        async void SpawnObjectsAsync()
        {
            for (int i = 0; i < _minCount; i++)
            {
                SpawnInPool(i);
                if (i % 50 == 0)
                    await Task.Yield();
            }

            _isSet = true;
            OnReady?.Invoke();
        }

        void SpawnInPool(int id = -1)
        {
            GameObject go = GameObject.Instantiate(_pooledPrefab, _parent);
            go.SetActive(false);

            go.name = $"{go.name.Remove(go.name.Length - 7)} [{id}]";
            _pooledObjects.Add(go);
        }

        void SetOnScene(int index, Transform parent = null, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion())
        {
            GameObject go = _pooledObjects[index];
            go.SetActive(true);
            go.transform.parent = parent;

            go.transform.SetPositionAndRotation(position, rotation);
        }

        public GameObject Get(Transform parent, Vector3 position, Quaternion rotation)
        {
            if (!_isSet)
                return null;

            if (_currentIndex == _pooledObjects.Count)
            {
                if (_pooledObjects.Count < _maxCount)
                    SpawnInPool();
                else
                    _currentIndex = 0;
            }

            SetOnScene(_currentIndex, parent, position, rotation);
            return _pooledObjects[_currentIndex++];
        }

        public GameObject Get() => Get(null, Vector3.zero, Quaternion.identity);

        public GameObject Get(Transform parent) => Get(parent, Vector3.zero, Quaternion.identity);

        public GameObject Get(Transform parent, Vector3 position) => Get(parent, position, Quaternion.identity);

        public bool TryRelease(GameObject item)
        {
            if (!_isSet)
                return false;

            int index = _pooledObjects.FindIndex(p => p == item);

            if (index == -1)
                return false;

            SetOnScene(index);
            item.SetActive(false);

            _pooledObjects.Swap(index, _pooledObjects.Count - 1);

            return true;
        }
    }
}
