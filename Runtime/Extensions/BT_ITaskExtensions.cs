using System.Collections.Generic;

namespace Common.BehaviourTrees
{
    public static class BT_ITaskExtensions
    {
        public static BT_ITask FindTaskByName(this BT_ITask self, string name)
        {
            if (self.Name == name)
            {
                return self;
            }

            if (self is BT_INode node)
            {
                foreach (var task in node.GetChildren())
                {
                    var result = FindTaskByName(task, name);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }

        public static IEnumerable<T> FindTasksByType<T>(this BT_ITask self)
            where T : class, BT_ITask
        {
            if (self is T result)
            {
                yield return result;
            }

            if (self is BT_INode node)
            {
                foreach (var task in node.GetChildren())
                {
                    foreach (var found in task.FindTasksByType<T>())
                    {
                        yield return found;
                    }
                }
            }
        }
    }
}