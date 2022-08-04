using System.Collections.Generic;
using UnityEngine;

namespace Euphrates
{
	public class BuyStation : MonoBehaviour
	{
		[SerializeField] FloatSO _withdrawDelay;

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != 6)
                return;


        }
    }
}
