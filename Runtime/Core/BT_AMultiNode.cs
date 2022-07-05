namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ATask"/> node with a multiple child tasks support
    /// </summary>
    public abstract class BT_AMultiNode : BT_ATask
    {
        protected int _current;
        protected BT_ITask[] _tasks;

        public BT_AMultiNode(string name = null) :
            base(name)
        {
        }

        public BT_ITask CurrentTask
        {
            get => _tasks[_current];
        }

        public virtual BT_ITask[] Tasks
        {
            get => _tasks;
            set => _tasks = value;
        }

        public virtual BT_ITask Task
        {
            get => _tasks[0];
            set => _tasks = new BT_ITask[] { value };
        }

        protected void AbortTasks()
        {
            if (_tasks != null)
            {
                for (int i = 0; i < _tasks.Length; ++i)
                {
                    _tasks[i].Abort();
                }
            }
        }

        protected override void OnStart()
        {
            base.OnStart();

            _current = 0;
        }

        protected override void OnFinish()
        {
            base.OnFinish();

            AbortTasks();
        }
    }
}
