using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Euphrates
{
    public class StackItemCountDisplayer : MonoBehaviour
    {
        [Header("Attritbutes")]
        [SerializeField, Min(-1)] int _itemMaxCount = -1;
        [SerializeField] Stacker _stack;
        [SerializeField] ItemSO _item;
        [SerializeField] Sprite _overrideImg;

        [Header("References"), Space]
        [SerializeReference] TextMeshProUGUI _text;
        [SerializeReference] Image _image;

        void Start()
        {

            if (_item.ItemIcon != null)
                _image.sprite = _item.ItemIcon;

            if (_overrideImg != null)
                _image.sprite = _overrideImg;

            UpdateText();
        }

        void OnEnable()
        {
            if (_stack == null)
                return;

            _stack.onItemAdded += UpdateText;
            _stack.onItemRemoved += UpdateText;
        }

        void OnDisable()
        {
            if (_stack == null)
                return;

            _stack.onItemAdded -= UpdateText;
            _stack.onItemRemoved -= UpdateText;
        }

        void UpdateText()
        {
            int itemCount = _stack.GetItemCount(_item);
            int itemMaxCount = _itemMaxCount == -1 ? _stack.GetSize() : _itemMaxCount;

            string txt = $"{itemCount}/{itemMaxCount}";
            _text.text = txt;
        }
    }
}
