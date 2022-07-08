namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ATask"/> node with a single child task support
    /// </summary>
    public abstract class BT_ASingleNode : BT_ATask
    {
        protected BT_ITask _task;

        public BT_ASingleNode(string name = null) :
            base(name)
        {
        }

        public virtual BT_ITask Task
        {
            get => _task;
            set => _task = value;
        }

        protected void AbortTask()
        {
            _task.Abort();
        }

        protected override BT_EStatus OnExecute()
        {
            return _task.Execute();
        }

        protected override void OnFinish()
        {
            base.OnFinish();

            AbortTask();
        }
    }
}
