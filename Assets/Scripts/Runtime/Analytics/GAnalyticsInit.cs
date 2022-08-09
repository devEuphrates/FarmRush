using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

namespace Euphrates
{
	public class GAnalyticsInit : MonoBehaviour
	{
        private void Start()
        {
            GameAnalytics.Initialize();
        }
    }
}
