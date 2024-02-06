using System;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ANode"/> which executes its child tasks in parallel until one fail or all succeed
    /// </summary>
    [Serializable]
    [BT_ItemMenu("Parallel", BT_MenuPath.Node, BT_MenuGroup.Core)]
    public sealed class BT_ParallelNode : BT_ANode
    {
        private bool _ran;

        public BT_ParallelNode() :
            base("Parallel")
        {
        }
        
        protected override void OnStart()
        {
            base.OnStart();

            _ran = false;
        }

        protected override BT_EStatus OnUpdate()
        {
            var status = BT_EStatus.Success;

            for (int i = _current; i < _children.Count; ++i)
            {
                var current = _children[i];
                if (
                    current.Status == BT_EStatus.Running ||
                    !_ran
                )
                {
                    var result = current.Update();

                    switch (result)
                    {
                        case BT_EStatus.Failure:
                            status = BT_EStatus.Failure;
                            break;

                        case BT_EStatus.Success:
                            if (_current == i)
                            {
                                _current += 1;
                            }
                            break;

                        case BT_EStatus.Running:
                            if (status != BT_EStatus.Failure)
                            {
                                status = BT_EStatus.Running;
                            }
                            break;
                    }
                }
            }

            _ran = true;

            return status;
        }
    }
}
