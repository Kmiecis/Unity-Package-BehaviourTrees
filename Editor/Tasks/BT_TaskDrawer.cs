using Common.BehaviourTrees;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CommonEditor.BehaviourTrees
{
    [CustomPropertyDrawer(typeof(BT_ATask), true)]
    public class BT_TaskDrawer : PropertyDrawer
    {
        private const float SpaceHeight = 2.0f;
        private const float IndentWidth = 10.0f;

        protected static readonly string[] TaskDrawOptions = new string[] { "_conditionals", "_decorators", "_services" };

        private readonly Dictionary<object, BT_Menu> _itemMenus;
        private readonly Dictionary<object, int> _drawMasks;

        public BT_TaskDrawer()
        {
            _itemMenus = new Dictionary<object, BT_Menu>();
            _drawMasks = new Dictionary<object, int>();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DrawLabel(ref position, label);
            DrawChoices(ref position, property);
            DrawChildren(ref position, property);
            DrawStatus(position, property);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (
                GetLabelAndChoicesHeight() +
                GetChildrenHeight(property)
            );
        }

        protected virtual string[] GetDrawOptions()
        {
            return TaskDrawOptions;
        }

        protected virtual string[] GetForcedDrawOptions()
        {
            return Array.Empty<string>();
        }

        private string[] GetPrettyDrawOptions()
        {
            var options = GetDrawOptions();
            var result = new string[options.Length];
            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = options[i].Replace("_", string.Empty).Capitalize();
            }
            return result;
        }

        private BT_Menu GetMenu(SerializedProperty property)
        {
            var key = property.propertyPath;
            if (!_itemMenus.TryGetValue(key, out var menu))
            {
                _itemMenus[key] = menu = new BT_Menu(property);
            }
            return menu;
        }

        private int GetDrawMask(SerializedProperty property)
        {
            var key = property.propertyPath;
            if (!_drawMasks.TryGetValue(key, out int mask))
            {
                var tempMask = 0;
                var index = 0;
                foreach (var child in GetChildren(property, true))
                {
                    if (child.arraySize > 0 ||
                        GetForcedDrawOptions().Contains(child.name))
                    {
                        var bit = 1 << index;
                        tempMask |= bit;
                    }
                    index++;
                }

                _drawMasks[key] = mask = tempMask;
            }
            return mask;
        }

        private void SetDrawMask(SerializedProperty property, int mask)
        {
            var key = property.propertyPath;
            _drawMasks[key] = mask;
        }

        private bool CanDraw(SerializedProperty property, SerializedProperty child)
        {
            var options = GetDrawOptions();
            var mask = GetDrawMask(property);
            var name = child.name;

            var index = Array.IndexOf(options, name);
            var bit = 1 << index;
            return (mask & bit) == bit;
        }

        private void DrawLabel(ref Rect position, GUIContent label)
        {
            position.x -= IndentWidth;
            position.width += IndentWidth;
            position.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.LabelField(position, label);
        }

        private void DrawChoices(ref Rect position, SerializedProperty property)
        {
            var options = GetPrettyDrawOptions();
            if (options.Length > 0)
            {
                var rect = position;
                rect.x += rect.width - (EditorGUIUtility.singleLineHeight + SpaceHeight);
                rect.width = EditorGUIUtility.singleLineHeight + SpaceHeight;

                var mask = GetDrawMask(property);
                var newMask = EditorGUI.MaskField(rect, mask, options);
                if (newMask != mask)
                {
                    SetDrawMask(property, newMask);
                }

                position.y += position.height + SpaceHeight;
            }
        }

        private float GetLabelAndChoicesHeight()
        {
            return EditorGUIUtility.singleLineHeight + SpaceHeight;
        }

        private void DrawChildren(ref Rect position, SerializedProperty property)
        {
            foreach (var child in GetChildren(property, true))
            {
                if (CanDraw(property, child))
                {
                    DrawProperty(ref position, child);

                    var menu = GetMenu(child);
                    menu.OnGUI();
                }
            }

            foreach (var child in GetChildren(property, false))
            {
                DrawProperty(ref position, child);
            }
        }

        private float GetChildrenHeight(SerializedProperty property)
        {
            var result = 0.0f;
            foreach (var child in GetChildren(property, true))
            {
                if (CanDraw(property, child))
                {
                    result += GetPropertyHeight(child);
                }
            }
            foreach (var child in GetChildren(property, false))
            {
                result += GetPropertyHeight(child);
            }
            return result;
        }

        private void DrawProperty(ref Rect position, SerializedProperty property)
        {
            position.height = EditorGUI.GetPropertyHeight(property, true);

            EditorGUI.PropertyField(position, property, true);

            position.y += position.height + SpaceHeight;
        }

        private float GetPropertyHeight(SerializedProperty property)
        {
            return EditorGUI.GetPropertyHeight(property, true) + SpaceHeight;
        }

        private BT_EStatus GetStatus(SerializedProperty property)
        {
            var statusProperty = property.FindPropertyRelative("_status");
            return statusProperty != null ? (BT_EStatus)statusProperty.enumValueIndex : BT_EStatus.Failure;
        }

        private float GetTimestamp(SerializedProperty property)
        {
            var timestampProperty = property.FindPropertyRelative("_updated");
            return timestampProperty != null ? timestampProperty.floatValue : -1.0f;
        }

        private Color GetStatusColor(BT_EStatus status)
        {
            switch (status)
            {
                case BT_EStatus.Failure: return Color.red;
                case BT_EStatus.Success: return Color.green;
                case BT_EStatus.Running: return Color.yellow;
            }
            return Color.black;
        }

        private void DrawStatus(Rect position, SerializedProperty property)
        {
            var status = GetStatus(property);
            var timestamp = GetTimestamp(property);

            var deltaTime = Time.realtimeSinceStartup - timestamp;
            var statusColor = GetStatusColor(status);
            var color = Color.Lerp(statusColor, Color.clear, deltaTime);

            position.width = 10.0f;
            position.x -= position.width + 3.0f;
            position.height = GetPropertyHeight(property, null) - EditorGUIUtility.singleLineHeight;
            position.y -= position.height;

            EditorGUI.DrawRect(position, color);
        }

        private IEnumerable<SerializedProperty> GetChildren(SerializedProperty property, bool drawn)
        {
            foreach (var child in property.GetChildren())
            {
                var contains = GetDrawOptions().Contains(child.name);

                if (drawn)
                {
                    if (contains)
                    {
                        yield return child;
                    }
                }
                else
                {
                    if (!contains)
                    {
                        yield return child;
                    }
                }
            }
        }
    }
}