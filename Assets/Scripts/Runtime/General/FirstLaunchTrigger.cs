using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Euphrates
{
	public class FirstLaunchTrigger : MonoBehaviour
	{
        [SerializeField] UnityEvent _onStart;
        private void Start()
        {
            if (!PlayerPrefs.HasKey("SaveData"))
                _onStart?.Invoke();
        }
    }
}
