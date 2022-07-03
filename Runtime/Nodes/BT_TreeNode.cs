namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ASingleNode"/> serving as a root of the tree
    /// </summary>
    public sealed class BT_TreeNode : BT_ASingleNode
    {
        public BT_TreeNode(string name = "Root") :
            base(name)
        {
        }
    }
}
