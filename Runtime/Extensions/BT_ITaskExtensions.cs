using System.Collections.Generic;

namespace Common.BehaviourTrees
{
    public static class BT_ITaskExtensions
    {
        public static IEnumerable<T> GetTasksByType<T>(this BT_ITask self)
        {
            if (self is T result)
            {
                yield return result;
            }

            if (self is BT_INode node)
            {
                foreach (var task in node.GetChildren())
                {
                    foreach (var found in task.GetTasksByType<T>())
                    {
                        yield return found;
                    }
                }
            }
        }
    }
}