using System.Collections.Generic;
using UnityEngine;

namespace Euphrates
{
    public class MoneySpawner : MonoBehaviour
    {
        [SerializeField] GameObject _cashPrefab;
        [SerializeField] int _moneyAmount = 10;
        [SerializeField] InfiniteStacker _stack;

        private void Start()
        {
            for (int i = 0; i < _moneyAmount; i++)
            {
                if (!_stack.CanAddItem())
                    return;

                GameObject go = Instantiate(_cashPrefab, transform.position, Quaternion.identity);
                Item it = go.GetComponent<Item>();

                _stack.AddItem(it);
            }
        }
    }
}
