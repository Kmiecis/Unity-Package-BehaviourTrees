using System;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ANode"/> which executes its child tasks in parallel until one fail or all succeed
    /// </summary>
    [Serializable]
    [BT_Menu("Parallel", BT_MenuPath.Node, BT_MenuGroup.Core)]
    public sealed class BT_ParallelNode : BT_ANode
    {
        private ulong _done;

        public BT_ParallelNode() :
            base("Parallel")
        {
        }
        
        protected override void OnStart()
        {
            base.OnStart();

            _done = 0U;
        }

        protected override BT_EStatus OnUpdate()
        {
            var status = BT_EStatus.Success;

            for (int i = _current; i < _children.Count; ++i)
            {
                if (!IsDone(i))
                {
                    var current = _children[i];

                    var result = current.Update();
                    switch (result)
                    {
                        case BT_EStatus.Failure:
                            status = BT_EStatus.Failure;
                            break;

                        case BT_EStatus.Success:
                            MarkDone(i);
                            if (_current == i)
                                _current += 1;
                            break;

                        case BT_EStatus.Running:
                            if (status != BT_EStatus.Failure)
                                status = BT_EStatus.Running;
                            break;
                    }
                }
            }

            return status;
        }

        private bool IsDone(int index)
        {
            var bit = 1U << index;
            return (_done & bit) == bit;
        }

        private void MarkDone(int index)
        {
            var bit = 1U << index;
            _done |= bit;
        }
    }
}
