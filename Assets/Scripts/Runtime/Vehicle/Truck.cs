using System;
using UnityEngine;

namespace Euphrates
{
    public class Truck : MonoBehaviour
	{
        [Header("References")]
        [SerializeField] FloatSO _truckMaxSpeed;
        [SerializeField] FloatSO _acclerationDistance;
        float _curSpeed = 0f;

        [Header("Variables"), Space]
        [SerializeField] float _distanceTreshold = 0.2f;

        [Header("Animation"), Space]
        [SerializeField] Animator _animator;
        [SerializeField] string _doorOpenAnim;
        [SerializeField] string _doorCloseAnim;

        VehicleState _curState = VehicleState.Idle;
        Vector3 _start;
        Vector3 _target;

        Orderer _orderer;

        event Action _onArrive;
        event Action _onOrderComplete;

        Stacker _moneyStack;

        private void Awake()
        {
            _orderer = GetComponent<Orderer>();
        }

        void OnEnable()
        {
            _orderer.OnOrderSatisfied += OrderSatisfied;
        }

        void OnDisable()
        {
            _orderer.OnOrderSatisfied -= OrderSatisfied;
        }

        void FixedUpdate()
        {
            if (_curState != VehicleState.Moving)
                return;

            Move();
        }

        public void SetMoneyStack(Stacker stack) => _moneyStack = stack;

        public void MoveAndOrder(Vector3 position, Action onOrderComplete = null)
        {
            MoveTo(position, () => { _orderer.RandomOrder(); DoorsState(true); });
            _onOrderComplete = onOrderComplete;
        }

        public void MoveTo(Vector3 pos, Action onArrive = null)
        {
            Vector3 dir = pos - transform.position;
			dir.Normalize();
			transform.forward = dir;

            _start = transform.position;
            _target = pos;
            
            _curSpeed = 0f;
            _curState = VehicleState.Moving;
            _onArrive = onArrive;
        }

        void Move()
        {
            
            _curSpeed = GetSpeed();
            transform.position += _curSpeed * Time.fixedDeltaTime * transform.forward;

            if (IsArrived())
                Arrived();
        }

        float GetSpeed()
        {
            float distT, distS;

            if ((distT = Vector3.Distance(transform.position, _target)) < _acclerationDistance)
                return Mathf.Clamp(_truckMaxSpeed - (_truckMaxSpeed / _acclerationDistance) * (_acclerationDistance - distT), 0.5f, _truckMaxSpeed);

            if ((distS = Vector3.Distance(transform.position, _start)) < _acclerationDistance)
                return Mathf.Clamp(_truckMaxSpeed - (_truckMaxSpeed / _acclerationDistance) * (_acclerationDistance - distS), 0.5f, _truckMaxSpeed);

            return _truckMaxSpeed;
        }

        bool IsArrived()
        {
            if (_curState != VehicleState.Moving || _target == null)
                return false;

            return Vector3.Distance(transform.position, _target) < _distanceTreshold;
        }

        void Arrived()
        {
            _curState = VehicleState.Idle;
            _onArrive?.Invoke();
        }

        public void DoorsState(bool open)
        {
            string anim = open ? _doorOpenAnim : _doorCloseAnim;
            _animator.SetTrigger(anim);
        }

        void OrderSatisfied()
        {
            PayOff();
            DoorsState(false);
            _onOrderComplete?.Invoke();
            _onOrderComplete = null;
            _orderer.ClearOrders();
        }

        void PayOff()
        {
            for (int i = 0; i < _orderer.GetOrdersWorth(); i++)
            {
                Item itm = SpawnManager.SpawnItem(3, null, transform.position);
                _moneyStack.AddItem(itm);
            }
        }

        public void ClearTruck()
        {
            _orderer.ClearItems();
        }
    }

	public enum VehicleState {Idle, Moving}
}
