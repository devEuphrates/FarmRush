using System;
using UnityEngine;

namespace Euphrates
{
    public class Item : MonoBehaviour
    {
        public ItemSO ItemInfo;

        bool _isFloating;
        float _startTime;

        float _animDuration;
        float _delayDuration;

        Vector3 _startPosition;
        Quaternion _startRotation;

        Vector3 _endPosition;
        Quaternion _endRotation;

        Vector3 _mPos;
        Quaternion _mRot;

        Action _onArrive = null;

        Transform _pivot = null;
        Transform _transform;

        void Awake() => _transform = transform;

        public void SnapToPos(Vector3 newPosition, Quaternion newRotation)
        {
            transform.position = newPosition;
            transform.rotation = newRotation;
        }

        public void FloatToPos(float duration, Vector3 newPosition, Quaternion newrotation, float delay = 0f, Action onArrive = null, Transform pivot = null)
        {
            _isFloating = true;

            _startTime = Time.time;
            _startPosition = _transform.position;
            _startRotation = _transform.rotation;

            if (pivot == null)
            {
                _endPosition = newPosition;
                _endRotation = newrotation;
            }
            else
            {
                _mPos = newPosition;
                _mRot = newrotation;

                (_endPosition, _endRotation) = RelativeTransform();
            }

            _pivot = pivot;

            _animDuration = duration;
            _delayDuration = delay;

            _onArrive = onArrive;
        }

        float _now;
        float _t;
        float _step;
        void FixedUpdate()
        {
            if (!_isFloating)
                return;

            _now = Time.time;
            if (_now - _startTime < _delayDuration)
                return;

            (Vector3 ep, Quaternion eh) = RelativeTransform();

            if (_now >= _startTime + _delayDuration + _animDuration)
            {
                _isFloating = false;

                _onArrive?.Invoke();
                _onArrive = null;

                if (_pivot != null)
                    SnapToPos(ep, eh);
                else
                    SnapToPos(_endPosition, _endRotation);

                _pivot = null;

                return;
            }

            _t = _now - _startTime - _delayDuration;
            _step = _t / _animDuration;

            if (_pivot != null)
            {
                _endPosition = ep;
                _endRotation = eh;
            }

            _transform.position = Vector3.Lerp(_startPosition, _endPosition, _step);
            _transform.rotation = Quaternion.Lerp(_startRotation, _endRotation, _step);

            //_transform.localPosition = Vector3.Lerp(_startPosition, _endPosition, _step);
            //_transform.localEulerAngles = Vector3.Lerp(_startRotation, _endRotation, _step);
        }

        (Vector3 pos, Quaternion rot) RelativeTransform()
        {
            if (_pivot == null)
                return (Vector3.zero, Quaternion.identity);

            return (_pivot.TransformPoint(_mPos), _mRot * _pivot.rotation);
        }
    }
}
