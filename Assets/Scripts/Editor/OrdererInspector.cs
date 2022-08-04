using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Euphrates
{
	[CustomEditor(typeof(Orderer))]
	public class OrdererInspector : Editor
	{
        Orderer _target;

        void OnEnable()
        {
            _target = (Orderer)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space(25);

            if (GUILayout.Button("Random Order"))
                _target.RandomOrder();

            if (GUILayout.Button("Clear Order"))
                _target.ClearOrders();
            
        }
    }
}
