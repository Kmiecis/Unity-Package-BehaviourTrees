using UnityEngine;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// Display a field as read-only in the inspector.
    /// CustomPropertyDrawers will not work when this attribute is used.
    /// </summary>
    public class ReadOnlyAttribute : PropertyAttribute
    {
    }
}
