namespace Common.BehaviourTrees
{
    public static class BT_ITaskExtensions
    {
        public static BT_ITask FindTaskByName(this BT_ITask self, string name)
        {
            if (self == null)
            {
                return null;
            }

            if (self.Name == name)
            {
                return self;
            }

            if (self is BT_INode node)
            {
                var tasks = node.Tasks;
                for (int i = 0; i < tasks.Length; ++i)
                {
                    var task = tasks[i];

                    var result = FindTaskByName(task, name);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }

        public static T FindTaskByType<T>(this BT_ITask self)
            where T : class, BT_ITask
        {
            if (self == null)
            {
                return null;
            }

            if (self is T result)
            {
                return result;
            }

            if (self is BT_INode node)
            {
                var tasks = node.Tasks;
                for (int i = 0; i < tasks.Length; ++i)
                {
                    var task = tasks[i];

                    result = FindTaskByType<T>(task);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }
    }
}