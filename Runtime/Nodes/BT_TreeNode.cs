namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ANode"/> serving as a root of the tree
    /// </summary>
    public sealed class BT_TreeNode : BT_ANode
    {
        public BT_TreeNode(string name = "Root") :
            base(name)
        {
        }

        protected override BT_EStatus OnUpdate()
        {
            return Current.Update();
        }
    }
}
