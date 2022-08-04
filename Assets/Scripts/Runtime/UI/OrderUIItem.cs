using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Euphrates
{
    public class OrderUIItem : MonoBehaviour
    {
        [SerializeField] Image _icon;
        [SerializeField] TextMeshProUGUI _countText;
        [SerializeField] CanvasGroup _canvasGroup;

        [Header("Animations"), Space]
        [SerializeField] FloatSO _animationDuration;
        [SerializeField] AnimationCurveSO _fadeAnim;
        [SerializeField] AnimationCurveSO _scaleAnim;

        OrderAnim _curAnim = OrderAnim.None;

        ItemOrder _order;

        float _timePassed = 0f;
        void Update()
        {
            if (_curAnim == OrderAnim.None)
                return;

            if ((_timePassed += Time.deltaTime) >= _animationDuration)
            {
                if (_curAnim == OrderAnim.Disapperaing)
                    DisableItem();

                _curAnim = OrderAnim.None;
                _timePassed = 0f;
            }

            float t;

            switch (_curAnim)
            {
                case OrderAnim.Disapperaing:
                    t = 1f - (_timePassed / _animationDuration);
                    break;

                case OrderAnim.Showing:
                    t = _timePassed / _animationDuration;
                    break;

                default:
                    return;
            }

            float stepFade = _fadeAnim.Value.Evaluate(t);
            _canvasGroup.alpha = stepFade;
            
            float stepScale = _scaleAnim.Value.Evaluate(t);
            transform.localScale = new Vector3(stepScale, stepScale, stepScale);
        }

        public void SetOrder(ItemOrder order)
        {
            gameObject.SetActive(true);
            _order = order;
            UpdateOrder();

            ShowItem();
        }

        public void UpdateOrder()
        {
            if (_order == null)
                return;

            _icon.sprite = _order.OrderedItem.ItemIcon;
            _countText.text = $"{_order.SatisfiedAmount}/{_order.ExpectedAmount}";
        }

        public void DisableItem()
        {
            _order = null;
            _icon.sprite = null;
            _countText.text = "";
            gameObject.SetActive(false);
        }

        public void ShowItem()
        {
            transform.localScale = Vector3.zero;
            _canvasGroup.alpha = 0f;

            _curAnim = OrderAnim.Showing;
        }

        public void HideItem()
        {
            transform.localScale = Vector3.one;
            _canvasGroup.alpha = 1f;

            _curAnim = OrderAnim.Disapperaing;
        }
    }

    enum OrderAnim { None, Showing, Disapperaing };
}
