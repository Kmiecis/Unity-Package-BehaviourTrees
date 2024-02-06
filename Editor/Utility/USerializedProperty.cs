using System;
using System.Reflection;
using UnityEditor;

namespace CommonEditor.BehaviourTrees
{
    internal static class USerializedProperty
    {
        private delegate bool PropertyEquals(SerializedProperty x, SerializedProperty y);

        private static PropertyEquals EqualContentsInternal = (PropertyEquals)
            typeof(SerializedProperty)
            .GetMethod("EqualContentsInternal", BindingFlags.Static | BindingFlags.NonPublic)
            .CreateDelegate(typeof(PropertyEquals));

        private static FieldInfo NativePropertyPtrField =
            typeof(SerializedProperty)
            .GetField("m_NativePropertyPtr", BindingFlags.Instance | BindingFlags.NonPublic);

        // A solution to SerializedProperty.EqualContents crashing Unity while iterating
        public static bool EqualContentsNoSync(SerializedProperty x, SerializedProperty y)
        {
            if (x == null)
            {
                return y == null || GetNativePropertyPtr(y) == IntPtr.Zero;
            }
            if (y == null)
            {
                return x == null || GetNativePropertyPtr(x) == IntPtr.Zero;
            }
            return EqualContentsInternal(x, y);
        }

        private static IntPtr GetNativePropertyPtr(SerializedProperty a)
        {
            return (IntPtr)NativePropertyPtrField.GetValue(a);
        }
    }
}