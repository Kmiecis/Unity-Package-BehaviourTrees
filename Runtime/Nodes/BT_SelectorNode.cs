using System;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ANode"/> which executes its child tasks in sequence until one succeed or all fail
    /// </summary>
    [Serializable]
    [BT_ItemMenu("Selector", BT_MenuPath.Node, BT_MenuGroup.Core)]
    public sealed class BT_SelectorNode : BT_ANode
    {
        public BT_SelectorNode() :
            base("Selector")
        {
        }
        
        protected override BT_EStatus OnUpdate()
        {
            for (; _current < _children.Count; ++_current)
            {
                var current = _children[_current];
                var result = current.Update();

                if (result != BT_EStatus.Failure)
                {
                    return result;
                }
            }

            _current -= 1;
            return BT_EStatus.Failure;
        }
    }
}
