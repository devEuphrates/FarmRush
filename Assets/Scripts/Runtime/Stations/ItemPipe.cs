using System.Collections.Generic;
using UnityEngine;

namespace Euphrates
{
	public class ItemPipe : MonoBehaviour
	{
        [SerializeField] ItemSO _spawnedItem;
		[SerializeField] Stacker _ownStack;
		[SerializeField] Transform _spawnPoint;
		[SerializeField] FloatSO _spawnDuration;

        bool _spawning = true;

        void OnEnable()
        {
            _ownStack.onItemAdded += OnItemAdded;
            _ownStack.onItemRemoved += OnItemRemoved;
        }

        void OnDisable()
        {
            _ownStack.onItemAdded -= OnItemAdded;
            _ownStack.onItemRemoved -= OnItemRemoved;
        }

        float _timePassed = 0f;
        void Update()
        {
            if (!_spawning)
                return;

            if ((_timePassed += Time.deltaTime) >= _spawnDuration)
            {
                _timePassed = 0f;

                if (!_ownStack.CanAddItem())
                {
                    _spawning = false;
                    return;
                }

                Item itm = SpawnManager.SpawnItem(_spawnedItem.ItemID, null, _spawnPoint.position, _spawnPoint.rotation);
                _ownStack.AddItem(itm);
            }
        }

        void OnItemAdded()
        {
            if (_ownStack.CanAddItem())
                return;

            _spawning = false;
        }

        void OnItemRemoved()
        {
            if (!_ownStack.CanAddItem())
                return;

            _spawning = true;
        }
    }
}
