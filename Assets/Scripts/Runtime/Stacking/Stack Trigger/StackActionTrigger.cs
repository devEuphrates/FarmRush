using System;
using UnityEngine;

namespace Euphrates
{
    [RequireComponent(typeof(BoxCollider))]
    public class StackActionTrigger : MonoBehaviour
    {
        [SerializeField] protected bool _drawGizmos;
        [Header("Stack and Action Attributes")]
        [SerializeField] protected Stacker _ownStack;
        [SerializeField] protected FloatSO _actionDelay;

        public event Action onAction;

        protected Stacker _stack;
        protected GameObject _stackOwner;

        protected BoxCollider _collider;

        void OnEnable() => _collider = GetComponent<BoxCollider>();

        void OnTriggerEnter(Collider other)
        {
            if (!other.transform.TryGetComponent<Stacker>(out _stack) || _stack == _ownStack)
                return;

            _stackOwner = other.gameObject;
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject != _stackOwner)
                return;

            _stackOwner = null;
            _stack = null;
        }


        float _timePassed = 0f;
        void Update()
        {
            if (_stackOwner == null)
                return;

            _timePassed += Time.deltaTime;
            if (_timePassed < _actionDelay)
                return;

            _timePassed = 0f;
            DoAction();
        }

        protected virtual void DoAction() => onAction?.Invoke();

        protected virtual void OnDrawGizmos()
        {
            if (!_drawGizmos)
                return;

            if (_collider == null)
                _collider = GetComponent<BoxCollider>();

            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(_collider.center, _collider.size);
        }
    }
}
