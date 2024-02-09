using System;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ANode"/> which executes its child tasks in sequence until one fail or all succeed
    /// </summary>
    [Serializable]
    [BT_Menu("Sequence", BT_MenuPath.Node, BT_MenuGroup.Core)]
    public sealed class BT_SequenceNode : BT_ANode
    {
        public BT_SequenceNode() :
            base("Sequence")
        {
        }
        
        protected override BT_EStatus OnUpdate()
        {
            for (; _current < _children.Count; ++_current)
            {
                var current = _children[_current];
                var result = current.Update();

                if (result != BT_EStatus.Success)
                {
                    return result;
                }
            }

            _current -= 1;
            return BT_EStatus.Success;
        }
    }
}
