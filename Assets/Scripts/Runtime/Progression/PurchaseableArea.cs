using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Euphrates
{
    public class PurchaseableArea : StackActionTrigger
	{
		[SerializeField] IntSO _cash;
        [SerializeField] FloatSO _cashFloatDuraton;
        [SerializeField] UnityEvent _onBuy;
        [SerializeField] int _price = 10;
        [SerializeField] TextMeshProUGUI _priceText;

        bool _done = false;

        void Start()
        {
            UpdateText();
        }

        protected override void DoAction()
        {
            if (_done || _cash <= 0)
                return;

            _cash.Value--;
            Item cashItem = SpawnManager.SpawnItem(3);
            cashItem.transform.position = _stackOwner.transform.position;
            cashItem.FloatToPos(_cashFloatDuraton, transform.position, Quaternion.identity, 0f, () => SpawnManager.ReleaseItem(cashItem));

            DecreasePrice();
            UpdateText();

            base.DoAction();
        }

        void DecreasePrice()
        {
            if (--_price > 0)
                return;

            _done = true;
            _onBuy?.Invoke();
        }

        void UpdateText() => _priceText.text = _price.ToString();
    }
}
