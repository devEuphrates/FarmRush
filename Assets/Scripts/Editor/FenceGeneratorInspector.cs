using UnityEditor;
using UnityEngine;

namespace Euphrates
{
    [CustomEditor(typeof(FenceGenerator))]
	public class FenceGeneratorInspector : Editor
	{
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Generate"))
                ((FenceGenerator)target).BuildFences();
        }
    }
}
