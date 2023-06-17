namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ATask"/> node with a multiple child tasks support
    /// </summary>
    public abstract class BT_AMultiNode : BT_ATask
    {
        protected int _current;
        protected BT_ITask[] _tasks;

        public BT_ITask CurrentTask
            => _tasks[_current];

        public BT_ITask[] Tasks
            => _tasks;

        public BT_AMultiNode(string name = null) :
            base(name)
        {
        }

        public BT_AMultiNode WithTasks(params BT_ITask[] tasks)
        {
            _tasks = tasks;
            return this;
        }
        
        protected override void OnStart()
        {
            _current = 0;
        }

        protected override void OnFinish()
        {
            AbortTasks();
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
    }
}
