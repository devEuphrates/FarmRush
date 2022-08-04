using UnityEngine;
using UnityEngine.Events;

namespace Euphrates
{
	[System.Serializable]
    public class SOVariable<T> : ScriptableObject
	{
		public T Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
				OnChange?.Invoke();
			}
		}

		[SerializeField] T _value;
		public event UnityAction OnChange;
	}
}