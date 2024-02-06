using System;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ANode"/> serving as a root of the tree
    /// </summary>
    [Serializable]
    [BT_ItemMenu("Root", BT_MenuPath.Node, BT_MenuGroup.Core)]
    public sealed class BT_RootNode : BT_ANode
    {
        public BT_RootNode() :
            base("Root")
        {
        }

        protected override BT_EStatus OnUpdate()
        {
            if (_current < _children.Count)
            {
                var current = _children[_current];
                return current.Update();
            }

            return BT_EStatus.Failure;
        }
    }
}
