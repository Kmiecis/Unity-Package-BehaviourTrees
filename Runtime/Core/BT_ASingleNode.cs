namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ATask"/> node with a single child task support
    /// </summary>
    public abstract class BT_ASingleNode : BT_ATask
    {
        protected BT_ITask _task;

        public BT_ITask Task
            => _task;

        public BT_ASingleNode(string name = null) :
            base(name)
        {
        }

        public BT_ASingleNode WithTask(BT_ITask task)
        {
            _task = task;
            return this;
        }

        protected override BT_EStatus OnUpdate()
        {
            return _task.Update();
        }

        protected override void OnFinish()
        {
            AbortTask();
        }

        protected void AbortTask()
        {
            _task.Abort();
        }
    }
}
