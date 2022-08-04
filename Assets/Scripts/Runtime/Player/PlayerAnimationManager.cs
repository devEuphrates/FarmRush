using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Euphrates
{
    public class PlayerAnimationManager : MonoBehaviour
    {
        [SerializeReference] CharacterController _controller;
        [SerializeReference] Animator _animator;

        [SerializeReference] Stacker _playerStack;
        [SerializeReference] Rig _handRig;
        [SerializeField] float _holdDuration;

        [SerializeField] string _runBoolName;

        AnimState _curState = AnimState.Idle;

        bool _holding = false;

        void OnEnable()
        {
            _playerStack.onItemAdded += ItemAdded;
            _playerStack.onItemRemoved += ItemRemove;
        }

        void OnDisable()
        {
            _playerStack.onItemAdded -= ItemAdded;
            _playerStack.onItemRemoved -= ItemRemove;
        }

        void Update()
        {
            UpdateRun();
        }

        void FixedUpdate()
        {
            
        }

        void UpdateRun()
        {
            switch (_curState)
            {
                case AnimState.Idle:

                    if (_controller.velocity.magnitude > 1f)
                    {
                        _animator.SetBool(_runBoolName, true);
                        _curState = AnimState.Running;
                    }

                    break;

                case AnimState.Running:

                    if (_controller.velocity.magnitude < 1f)
                    {
                        _animator.SetBool(_runBoolName, false);
                        _curState = AnimState.Idle;
                    }

                    break;

                default:
                    break;
            }
        }

        void UpdateHands(bool holding)
        {
            _holding = holding;

            if (holding)
            {
                HandsHold();
                return;
            }

            HandsDrop();
        }

        async void HandsHold()
        {
            float start = Time.time;

            float passed, t;
            while ((passed = Time.time - start) < _holdDuration)
            {
                t = passed / _holdDuration;
                _handRig.weight = t;
                await Task.Yield();
            }

            _handRig.weight = 1f;
        }

        async void HandsDrop()
        {
            float start = Time.time;

            float passed, t;
            while ((passed = Time.time - start) < _holdDuration)
            {
                t = 1f - (passed / _holdDuration);
                _handRig.weight = t;
                await Task.Yield();
            }

            _handRig.weight = 0f;
        }

        void ItemAdded()
        {
            if (_holding)
                return;
            
            UpdateHands(true);
        }

        void ItemRemove()
        {
            if (_playerStack.GetItemCount() != 0)
                return;

            UpdateHands(false);
        }

        enum AnimState { Idle, Running }
    }
}
