using UnityEngine;

namespace Euphrates
{
    [System.Serializable]
    public struct ObjectSaveData
    {
        public string UName;
        public bool IsActive;
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;
    }
}
