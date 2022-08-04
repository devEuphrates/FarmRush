using UnityEngine;

namespace Euphrates
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
        static T _instance;
        public static T Instance
        {
			get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject { name = typeof(T).Name };
                    _instance = go.AddComponent<T>();
                }

                return _instance;
            }

			set
            {
                Destroy(_instance);
                _instance = value;
            }
        }

        /// <summary>
        /// Must use "base.Awake();" if overwritten or singleton won't work.
        /// </summary>
        protected virtual void Awake()
        {
            if (_instance != null)
                Destroy(this);

            _instance = this as T;
        }
    }
}
