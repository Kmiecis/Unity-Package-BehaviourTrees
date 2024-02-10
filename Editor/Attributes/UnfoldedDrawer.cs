using Common.BehaviourTrees;
using UnityEditor;
using UnityEngine;

namespace CommonEditor.BehaviourTrees
{
    [CustomPropertyDrawer(typeof(UnfoldedAttribute), true)]
    public class UnfoldedDrawer : PropertyDrawer
    {
        private const float SpaceHeight = 2.0f;
        private const float IndentWidth = 10.0f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DrawLabel(ref position, label);
            DrawChildren(ref position, property);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (
                GetLabelHeight() +
                GetChildrenHeight(property)
            );
        }

        private void DrawLabel(ref Rect position, GUIContent label)
        {
            position.x -= IndentWidth;
            position.width += IndentWidth;
            position.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.LabelField(position, label);

            position.y += position.height + SpaceHeight;
        }

        private float GetLabelHeight()
        {
            return EditorGUIUtility.singleLineHeight + SpaceHeight;
        }

        private void DrawChildren(ref Rect position, SerializedProperty property)
        {
            foreach (var child in property.GetChildren())
            {
                position.height = EditorGUI.GetPropertyHeight(child, true);

                EditorGUI.PropertyField(position, child, true);

                position.y += position.height + SpaceHeight;
            }
        }

        private float GetChildrenHeight(SerializedProperty property)
        {
            var result = 0.0f;
            foreach (var child in property.GetChildren())
            {
                result += EditorGUI.GetPropertyHeight(child, true) + SpaceHeight;
            }
            return result;
        }
    }
}