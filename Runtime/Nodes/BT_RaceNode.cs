using System;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ANode"/> which executes its child tasks in parallel until one succeed or all fail
    /// </summary>
    [Serializable]
    [BT_Menu("Race", BT_MenuPath.Node, BT_MenuGroup.Core)]
    public sealed class BT_RaceNode : BT_ANode
    {
        private ulong _done;

        public BT_RaceNode() :
            base("Race")
        {
        }
        
        protected override void OnStart()
        {
            base.OnStart();

            _done = 0U;
        }

        protected override BT_EStatus OnUpdate()
        {
            var status = BT_EStatus.Failure;

            for (int i = _current; i < _children.Count; ++i)
            {
                if (!IsDone(i))
                {
                    var current = _children[i];

                    var result = current.Update();
                    switch (result)
                    {
                        case BT_EStatus.Failure:
                            MarkDone(i);
                            if (_current == i)
                                _current += 1;
                            break;

                        case BT_EStatus.Success:
                            status = BT_EStatus.Success;
                            break;

                        case BT_EStatus.Running:
                            if (status != BT_EStatus.Success)
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
