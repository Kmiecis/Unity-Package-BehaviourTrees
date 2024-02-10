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