using Common.BehaviourTrees;
using UnityEditor;
using UnityEngine;

namespace CommonEditor.BehaviourTrees
{
    [CustomPropertyDrawer(typeof(RuntimeOnlyAttribute))]
    public class RuntimeOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (Application.isPlaying)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (Application.isPlaying)
            {
                return EditorGUI.GetPropertyHeight(property, label, true);
            }
            return 0.0f;
        }
    }
}