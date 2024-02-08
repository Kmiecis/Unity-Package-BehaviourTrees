using Common.BehaviourTrees;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace CommonEditor.BehaviourTrees
{
    [CustomEditor(typeof(BT_Blackboard))]
    public class BT_BlackboardEditor : Editor
    {
        private static readonly string[] RootDrawOptions = new string[] { "_conditionals", "_decorators", "_services", "_children" };
        private static readonly string[] RootForcedDrawOptions = new string[] { "_children" };

        private SerializedProperty _rootProperty;

        private Dictionary<string, BT_ItemMenu> _itemMenus;
        private int _drawMask;

        public BT_BlackboardEditor()
        {
            _itemMenus = new Dictionary<string, BT_ItemMenu>();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawChoices();
            DrawProperties();

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual string[] GetDrawOptions()
        {
            return RootDrawOptions;
        }

        protected virtual string[] GetForcedDrawOptions()
        {
            return RootForcedDrawOptions;
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

        private void ReadMenus()
        {
            foreach (var child in GetChildren(true))
            {
                _itemMenus[child.name] = new BT_ItemMenu(child);
            }
        }

        private void ReadDrawMask()
        {
            var tempMask = 0;

            var index = 0;
            foreach (var child in GetChildren(true))
            {
                if (child.arraySize > 0 ||
                    GetForcedDrawOptions().Contains(child.name))
                {
                    var bit = 1 << index;
                    tempMask |= bit;
                }
                index++;
            }

            _drawMask = tempMask;
        }

        private void DrawChoices()
        {
            var options = GetPrettyDrawOptions();
            if (options.Length > 0)
            {
                _drawMask = EditorGUILayout.MaskField(_drawMask, options);
            }
        }

        private bool CanDraw(SerializedProperty property)
        {
            var options = GetDrawOptions();
            var name = property.name;

            var index = Array.IndexOf(options, name);
            var bit = 1 << index;
            return (_drawMask & bit) == bit;
        }

        private void DrawProperties()
        {
            foreach (var child in GetChildren(true))
            {
                if (CanDraw(child))
                {
                    DrawProperty(child);

                    var menu = _itemMenus[child.name];
                    menu.OnGUI();
                }
            }

            foreach (var child in GetChildren(false))
            {
                DrawProperty(child);
            }
        }

        private void DrawProperty(SerializedProperty property)
        {
            EditorGUILayout.PropertyField(property, true);
        }

        private IEnumerable<SerializedProperty> GetChildren(bool drawn)
        {
            foreach (var child in _rootProperty.GetChildren())
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

        private void OnEnable()
        {
            _rootProperty = serializedObject.FindProperty("_root");

            ReadMenus();
            ReadDrawMask();
        }
    }
}