using UnityEngine;

namespace Euphrates
{
    public class SaveState : MonoBehaviour
    {
        public bool SaveActiveState => _saveActiveState;
        public bool SaveTransform => _saveTransform;

        [SerializeField] bool _saveTransform = true;
        [SerializeField] bool _saveActiveState = true;
    }
}
