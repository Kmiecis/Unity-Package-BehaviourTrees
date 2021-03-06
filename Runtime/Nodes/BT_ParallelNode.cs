namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_AMultiNode"/> which executes its child tasks in parallel until one fail or all succeed
    /// </summary>
    public sealed class BT_ParallelNode : BT_AMultiNode
    {
        private bool _ran;

        public BT_ParallelNode(string name = "") :
            base(name)
        {
        }
        
        protected override void OnStart()
        {
            base.OnStart();

            _ran = false;
        }

        protected override BT_EStatus OnExecute()
        {
            var status = BT_EStatus.Success;

            for (int i = _current; i < _tasks.Length; ++i)
            {
                var current = _tasks[i];
                if (
                    current.Status == BT_EStatus.Running ||
                    !_ran
                )
                {
                    var result = current.Execute();

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
