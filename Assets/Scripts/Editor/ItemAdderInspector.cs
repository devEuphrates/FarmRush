using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Euphrates
{
    [CustomEditor(typeof(ItemAdder))]
    public class ItemAdderInspector : Editor
    {
        SerializedObject _serializedObject;

        SerializedProperty _stackProperty;
        ItemAdder _target;

        ItemSO _addedItem;
        int _addedAmount = 1;

        void OnEnable()
        {
            _serializedObject = serializedObject;
            _stackProperty = _serializedObject.FindProperty("_stack");
            _target = target as ItemAdder;
        }

        public override void OnInspectorGUI()
        {
            _serializedObject.Update();

            EditorGUILayout.PropertyField(_stackProperty);
            GUILayout.Space(10f);

            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                if (_target.Items == null || _target.Items.Count == 0)
                {
                    GUIStyle noItemStyle = EditorStyles.miniBoldLabel;
                    EditorGUILayout.LabelField("Holding No Items", noItemStyle);
                }
                else
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Label("Item", GUILayout.MinWidth(20f));
                        GUILayout.Label("Amount", GUILayout.MinWidth(20f), GUILayout.MaxWidth(120f));
                    }

                    for (int i = _target.Items.Count - 1; i >= 0; i--)
                    {
                        using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
                        {
                            GUILayout.Label(_target.Items[i].AddedItem.ItemName, GUILayout.MinWidth(20f));
                            _target.Items[i].AddedAmount = EditorGUILayout.IntField(_target.Items[i].AddedAmount, GUILayout.Width(50f));

                            if (GUILayout.Button("Remove", GUILayout.Width(60f)))
                            {
                                _target.Items.RemoveAt(i);
                                EditorUtility.SetDirty(target);
                            }
                        }
                    }
                }
            }

            if (GUILayout.Button("Spawn"))
                _target.SpawnItems();

            GUILayout.Space(10f);
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUILayout.Label("Add New Item");
                using (new GUILayout.HorizontalScope())
                {
                    _addedItem = EditorGUILayout.ObjectField(_addedItem, typeof(ItemSO), true, GUILayout.MinWidth(20f)) as ItemSO;
                    _addedAmount = EditorGUILayout.IntField(_addedAmount, GUILayout.Width(50f));

                    if (_addedItem != null && GUILayout.Button("Add", GUILayout.Width(60f)))
                    {
                        _target.Items.Add(new ItemAdderSpawnInfo() { AddedItem = _addedItem, AddedAmount = _addedAmount });
                        _addedItem = null;
                        EditorUtility.SetDirty(target);

                    }
                }
            }

            _serializedObject.ApplyModifiedProperties();
        }
    }
}
