using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using System.Linq;
using Common.BehaviourTrees;
using System.Reflection;

namespace CommonEditor.BehaviourTrees
{
    public class BT_ItemMenu
    {
        private class EntryData
        {
            public string menuPath;
            public string fileName;
            public Type type;
        }

        private readonly SerializedProperty _property;
        private readonly Type _type;

        private int _count;

        public BT_ItemMenu(SerializedProperty property)
        {
            _property = property.Copy();

            var assembly = Assembly.Load($"{nameof(Common)}.{nameof(BehaviourTrees)}");
            _type = assembly.FindType(property.type);

            _count = property.arraySize;
        }

        public void OnGUI()
        {
            var current = _property.arraySize;
            if (current > _count)
            {
                var last = _property.GetLastArrayElement();

                if (last.managedReferenceValue == null)
                {
                    _property.DeleteLastArrayElement();

                    ShowAddMenu();
                }
                else
                {
                    var added = FindDuplicate();

                    if (added != null)
                    {
                        Replace(added);
                    }
                }
            }
            else
            {
                _count = current;
            }
        }

        private bool IsValidType(Type type)
        {
            return (
                type.HasInterface(_type) &&
                !type.IsInterface &&
                !type.IsAbstract
            );
        }

        private void ShowAddMenu()
        {
            var menu = new GenericMenu();

            var map = new Dictionary<int, List<EntryData>>();

            var types = AppDomain.CurrentDomain.FindTypes(IsValidType);
            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute<BT_ItemMenuAttribute>();

                var menuPath = attribute.GetMenuPathOrDefault();
                var fileName = attribute.GetFileNameOrDefault(type.Name);
                var group = attribute.GetGroupOrDefault();

                var data = new EntryData
                {
                    menuPath = menuPath,
                    fileName = fileName,
                    type = type
                };

                if (!map.TryGetValue(group, out var target))
                {
                    map[group] = target = new List<EntryData>();
                }
                target.Add(data);
            }

            var added = 0;

            var mapped = map.OrderBy(kv => kv.Key);
            foreach (var kv in mapped)
            {
                if (added > 0)
                {
                    menu.AddSeparator(string.Empty);
                }
                added += 1;

                foreach (var data in kv.Value)
                {
                    var path = $"{data.menuPath}/{data.fileName}";

                    menu.AddItem(new GUIContent(path), false, OnMenuAdd, data.type);
                }
            }

            menu.ShowAsContext();
        }

        private void OnMenuAdd(object type)
        {
            var instance = CreateObjectOfType((Type)type);

            var added = _property.AddArrayElement();
            added.isExpanded = true;
            added.managedReferenceValue = instance;

            _property.serializedObject.ApplyModifiedProperties();

            _count = _property.arraySize;
        }

        private void Replace(SerializedProperty item)
        {
            var oldValue = item.managedReferenceValue;

            var newValue = CreateObjectOfType(oldValue.GetType());
            EditorUtility.CopySerializedManagedFieldsOnly(oldValue, newValue);

            item.managedReferenceValue = newValue;
        }

        private object CreateObjectOfType(Type type)
        {
            return Activator.CreateInstance(type);
        }

        private SerializedProperty FindDuplicate()
        {
            var previous = (SerializedProperty)null;
            foreach (var child in _property.GetChildren())
            {
                if (Equals(previous, child))
                {
                    return child;
                }
                previous = child;
            }
            return previous;
        }
    }
}