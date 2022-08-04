using UnityEngine;

namespace Euphrates
{
    public class PlayerController : MonoBehaviour
    {
        CharacterController _controller;
        void Awake() => _controller = GetComponent<CharacterController>();

        Vector2 _initialPos = Vector2.zero;
        bool _firstDown = false;

        [SerializeField] float _speed = 10f;
        [SerializeField] float _maxInitialPointDist = 25f;

        void Update()
        {
            Vector3 moveVector = Vector3.zero;

            if (Input.GetMouseButtonUp(0))
            {
                _firstDown = false;
                _initialPos = Vector2.zero;
            }

            Vector2 mousePos = Input.mousePosition;

            if (!_firstDown && Input.GetMouseButtonDown(0))
            {
                _firstDown = true;
                _initialPos = mousePos;
            }

            if (_firstDown && Input.GetMouseButton(0))
            {
                if (Vector2.Distance(mousePos, _initialPos) > _maxInitialPointDist)
                    _initialPos = mousePos + (_initialPos - mousePos).normalized * _maxInitialPointDist;

                Vector2 slideVector = mousePos - _initialPos;
                moveVector = new Vector3(slideVector.x, 0f, slideVector.y).normalized;

                Vector3 forwardNrm = _controller.velocity.normalized;

                if (_controller.velocity.magnitude > 0f)
                    transform.forward = new Vector3(forwardNrm.x, 0f, forwardNrm.z);
            }

            _controller.SimpleMove(moveVector * _speed);
        }
    }
}
