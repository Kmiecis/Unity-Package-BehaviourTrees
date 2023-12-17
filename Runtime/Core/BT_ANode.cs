namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ATask"/> node with a multiple child tasks support
    /// </summary>
    public abstract class BT_ANode : BT_ATask, BT_INode
    {
        protected BT_ITask[] _tasks;
        protected int _current;

        public BT_ITask[] Tasks
            => _tasks;

        public BT_ITask Current
            => _tasks[_current];

        public BT_ANode(string name = null) :
            base(name)
        {
        }

        public BT_ANode WithTask(BT_ITask task)
        {
            _tasks = new BT_ITask[] { task };
            return this;
        }

        public BT_ANode WithTasks(params BT_ITask[] tasks)
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
