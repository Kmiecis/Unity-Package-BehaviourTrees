using System.Collections.Generic;

namespace Common.BehaviourTrees
{
    public static class BT_INodeExtensions
    {
        public static IEnumerable<T> GetChildrenByType<T>(this BT_INode self)
        {
            foreach (var child in self.GetChildren())
            {
                if (child is T result)
                {
                    yield return result;
                }
            }
        }
    }
}