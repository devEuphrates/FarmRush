using TMPro;
using UnityEngine;

namespace Euphrates
{
    public class TextUpdater : MonoBehaviour
    {
        [SerializeField] SOVarType _updateType = SOVarType.Integer;

        [SerializeField] IntSO _intVar;
        [SerializeField] IntSO _floatVar;

        [SerializeField] TextMeshProUGUI _text;

        void Start()
        {
            UpdateText();
        }

        void OnEnable()
        {
            if (_intVar != null)
                _intVar.OnChange += UpdateText;
            if (_floatVar != null)
                _floatVar.OnChange += UpdateText;
        }

        void OnDisable()
        {
            if (_intVar != null)
                _intVar.OnChange -= UpdateText;
            if (_floatVar != null)
                _floatVar.OnChange -= UpdateText;
        }

        void UpdateText()
        {
            switch (_updateType)
            {
                case SOVarType.Integer:
                    if (_intVar == null)
                        return;
                    _text.text = _intVar.Value.ToString();
                    break;
                case SOVarType.Float:
                    if (_floatVar == null)
                        return;
                    _text.text = _floatVar.Value.ToString();
                    break;
                default:
                    break;
            }
        }
    }

    public enum SOVarType { Integer, Float }
}
