using UnityEngine;

namespace Euphrates
{
    [RequireComponent(typeof(RectTransform))]
	public class SafeAreaResizer : MonoBehaviour
	{
        private void Awake()
        {
            var rec = GetComponent<RectTransform>();
            var safeArea = Screen.safeArea;

            var minAnchor = safeArea.position;
            var maxAnchor = minAnchor + safeArea.size;

            minAnchor.x /= Screen.width;
            minAnchor.y /= Screen.height;

            maxAnchor.x /= Screen.width;
            maxAnchor.y /= Screen.height;

            rec.anchorMin = minAnchor;
            rec.anchorMax = maxAnchor;
        }
    }
}
