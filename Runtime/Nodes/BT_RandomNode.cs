using System;
using Random = UnityEngine.Random;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ANode"/> which executes a random child task as long as it is running
    /// </summary>
    [Serializable]
    [BT_Menu("Random", BT_MenuPath.Node, BT_MenuGroup.Core)]
    public sealed class BT_RandomNode : BT_ANode
    {
        public BT_RandomNode() :
            base("Random")
        {
        }
        
        protected override void OnStart()
        {
            _current = Random.Range(0, _children.Count);
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
