using System.Collections.Generic;
using UnityEngine;

namespace Euphrates
{
    public class OrderDisplayer : MonoBehaviour
    {
        [SerializeField] List<OrderUIItem> _orderItems;
        [SerializeField] AnimationCurveSO _animation;

        bool _playing = false;
        float _duration;

        float _timePassed = 0f;
        void Update()
        {
            if (!_playing)
                return;

            if ((_timePassed += Time.deltaTime) >= _duration)
            {
                float end = _animation.Value.Evaluate(1f);
                transform.localScale = new Vector3(end, end, end);

                _playing = false;
                _timePassed = 0f;
                return;
            }

            float step = _timePassed / _duration;
            float val = _animation.Value.Evaluate(step);

            transform.localScale = new Vector3(val, val, val);
        }

        public void ShowSelf(float duration)
        {
            _duration = duration;
            _playing = true;
        }

        public void ShowOrders(List<ItemOrder> orders)
        {
            HideOrders();

            for (int i = 0; i < orders.Count; i++)
                _orderItems[i].SetOrder(orders[i]);
        }

        public void HideOrders()
        {
            foreach (var o1 in _orderItems)
                o1.HideItem();
        }

        public void UpdateOrders()
        {
            foreach (var item in _orderItems)
                item.UpdateOrder();
        }
    }
}
