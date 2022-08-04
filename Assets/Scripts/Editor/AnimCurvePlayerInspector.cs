using UnityEditor;
using UnityEngine;

namespace Euphrates.Editors
{
    [CustomEditor(typeof(AnimCurvePlayer))]
    [CanEditMultipleObjects]
    public class AnimCurvePlayerInspector : Editor
    {
        SerializedObject _serializedObject;

        AnimCurvePlayer _target;

        SerializedProperty _animDuration;
        SerializedProperty _keepEndTransform;

        SerializedProperty _doPositionAnim;
        SerializedProperty _positionAnim;
        SerializedProperty _additivePosition;

        SerializedProperty _doRotationAnim;
        SerializedProperty _rotationAnim;
        SerializedProperty _additiveRotation;

        SerializedProperty _doScaleAnim;
        SerializedProperty _scaleAnim;
        SerializedProperty _additiveScale;

        SerializedProperty _onAnimationEnd;

        private void OnEnable()
        {
            _serializedObject = serializedObject;
            _target = target as AnimCurvePlayer;

            _animDuration = _serializedObject.FindProperty("_animDuration");
            _keepEndTransform = _serializedObject.FindProperty("_keepEndTransform");

            _doPositionAnim = _serializedObject.FindProperty("_doPositionAnim");
            _positionAnim = _serializedObject.FindProperty("_positionAnim");
            _additivePosition = _serializedObject.FindProperty("_additivePosition");

            _doRotationAnim = _serializedObject.FindProperty("_doRotationAnim");
            _rotationAnim = _serializedObject.FindProperty("_rotationAnim");
            _additiveRotation = _serializedObject.FindProperty("_additiveRotation");

            _doScaleAnim = _serializedObject.FindProperty("_doScaleAnim");
            _scaleAnim = _serializedObject.FindProperty("_scaleAnim");
            _additiveScale = _serializedObject.FindProperty("_additiveScale");

            _onAnimationEnd = _serializedObject.FindProperty("OnAnimationEnd");
        }

        public override void OnInspectorGUI()
        {
            _serializedObject.Update();

            EditorGUILayout.PropertyField(_animDuration);
            EditorGUILayout.PropertyField(_keepEndTransform);


            EditorGUILayout.Space(10);
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.PropertyField(_doPositionAnim);

                if (_doPositionAnim.boolValue)
                {

                    EditorGUILayout.PropertyField(_positionAnim);

                    EditorGUILayout.Space(5);

                    EditorGUILayout.PropertyField(_additivePosition);
                }
            }

            EditorGUILayout.Space(10);

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.PropertyField(_doRotationAnim);

                if (_doRotationAnim.boolValue)
                {

                    EditorGUILayout.PropertyField(_rotationAnim);

                    EditorGUILayout.Space(5);

                    EditorGUILayout.PropertyField(_additiveRotation);
                }
            }

            EditorGUILayout.Space(10);

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.PropertyField(_doScaleAnim);

                if (_doScaleAnim.boolValue)
                {

                    EditorGUILayout.PropertyField(_scaleAnim);

                    EditorGUILayout.Space(5);

                    EditorGUILayout.PropertyField(_additiveScale);
                }
            }

            EditorGUILayout.Space(20);
            //EditorGUILayout.PropertyField(_onAnimationEnd);

            if (Application.isPlaying)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    bool isPlaying = _target.AnimationState();

                    if (isPlaying)
                        GUI.enabled = false;

                    if (GUILayout.Button("Play", GUILayout.MaxWidth(100000)))
                        _target.Play();

                    GUI.enabled = true;

                    string animState = isPlaying ? "Playing" : "Paused";
                    EditorGUILayout.LabelField(animState, GUILayout.Width(50));
                }
            }

            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(_onAnimationEnd);

            _serializedObject.ApplyModifiedProperties();
        }
    }
}
