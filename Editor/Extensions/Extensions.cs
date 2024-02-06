using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace CommonEditor.BehaviourTrees
{
    internal static class Extensions
    {
        #region AppDomain
        public static IEnumerable<Type> FindTypes(this AppDomain self, Predicate<Type> match)
        {
            foreach (var assembly in self.GetAssemblies())
            {
                foreach (var type in assembly.FindTypes(match))
                {
                    yield return type;
                }
            }
        }
        #endregion

        #region Assembly
        public static IEnumerable<Type> FindTypes(this Assembly self, Predicate<Type> match)
        {
            foreach (var type in self.GetTypes())
            {
                if (match(type))
                {
                    yield return type;
                }
            }
        }

        public static Type FindType(this Assembly self, string name)
        {
            foreach (var type in self.GetTypes())
            {
                if (type.Name == name ||
                    type.FullName == name)
                {
                    return type;
                }
            }
            return null;
        }
        #endregion

        #region SerializedProperty
        public static IEnumerable<SerializedProperty> GetChildren(this SerializedProperty self)
        {
            var iterator = self.Copy();
            var end = iterator.GetEndProperty();

            if (iterator.NextVisible(true))
            {
                do
                {
                    if (USerializedProperty.EqualContentsNoSync(iterator, end))
                    {
                        break;
                    }

                    yield return iterator;
                }
                while (iterator.NextVisible(false));
            }
        }

        public static SerializedProperty AddArrayElement(this SerializedProperty self)
        {
            int index = self.arraySize;
            self.InsertArrayElementAtIndex(index);
            return self.GetArrayElementAtIndex(index);
        }

        public static SerializedProperty GetLastArrayElement(this SerializedProperty self)
        {
            int index = self.arraySize - 1;
            return self.GetArrayElementAtIndex(index);
        }

        public static void DeleteLastArrayElement(this SerializedProperty self)
        {
            int index = self.arraySize - 1;
            self.DeleteArrayElementAtIndex(index);
        }

        public static object GetValue(this SerializedProperty self)
        {
            var sanitizedPath = self.propertyPath.Replace(".Array.data[", "[");
            object result = self.serializedObject.targetObject;

            var elements = sanitizedPath.Split('.');
            foreach (var element in elements)
            {
                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var strIndex = element.Substring(element.IndexOf("[")).Replace("[", string.Empty).Replace("]", string.Empty);
                    var index = int.Parse(strIndex);
                    result = GetValueFromList(result, elementName, index);
                }
                else
                {
                    result = GetValueFromType(result, element);
                }
            }
            return result;
        }

        private static object GetValueFromList(object source, string name, int index)
        {
            var list = GetValueFromType(source, name) as System.Collections.IList;
            if (list != null)
            {
                return list[index];
            }
            return null;
        }

        private static object GetValueFromType(object source, string name)
        {
            var type = source.GetType();
            while (type != null)
            {
                var field = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (field != null)
                {
                    return field.GetValue(source);
                }

                var property = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (property != null)
                {
                    return property.GetValue(source, null);
                }

                type = type.BaseType;
            }
            return null;
        }
        #endregion

        #region String
        public static string Capitalize(this string self)
        {
            if (string.IsNullOrEmpty(self))
                return self;
            if (self.Length == 1)
                return char.ToUpper(self[0]).ToString();
            else
                return char.ToUpper(self[0]) + self.Substring(1);
        }
        #endregion

        #region Type
        public static bool HasInterface(this Type self, Type target)
        {
            var interfaces = self.GetInterfaces();
            foreach (var item in interfaces)
            {
                if (Equals(item, target))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}