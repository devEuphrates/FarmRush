using UnityEngine;

namespace Euphrates
{
    public class FaceCamera : MonoBehaviour
	{
        Camera _camera;

        void Start() => _camera = Camera.allCameras[0];

        void LateUpdate() => transform.forward = _camera.transform.forward;
    }
}
