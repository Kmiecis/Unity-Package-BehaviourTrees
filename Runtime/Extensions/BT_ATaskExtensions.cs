namespace Common.BehaviourTrees
{
    public static class BT_ATaskExtensions
    {
        public static BT_ATask FindTaskByName(this BT_ATask self, string name)
        {
            if (self.Name == name)
            {
                return self;
            }

            if (self is BT_INode node)
            {
                foreach (var task in node.GetChildrenByType<BT_ATask>())
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
    }
}