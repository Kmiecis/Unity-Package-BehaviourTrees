namespace Common.BehaviourTrees
{
    /// <summary>
    /// Base <see cref="BT_IDecorator"/> implementation
    /// </summary>
    public abstract class BT_ADecorator : BT_IDecorator
    {
        protected string _name;
        protected bool _started;

        public BT_ADecorator(string name = null)
        {
            _name = name ?? GetType().Name;
        }

        public string Name
        {
            get => _name;
        }

        public void Start()
        {
            if (!_started)
            {
                OnStart();
                _started = true;
            }
        }

        protected virtual void OnStart()
        {
        }

        public abstract BT_EStatus Decorate(BT_EStatus status);

        public void Finish(BT_EStatus status)
        {
            if (_started)
            {
                OnFinish(status);
                _started = false;
            }
        }

        protected virtual void OnFinish(BT_EStatus status)
        {
        }

        public override string ToString()
        {
            return _name;
        }
    }

    /// <summary>
    /// <see cref="BT_ADecorator"/> implementation with build-in context <see cref="T"/> support of any type
    /// </summary>
    public abstract class BT_ADecorator<T> : BT_ADecorator
    {
        protected readonly T _context;

        public BT_ADecorator(T context, string name = null) :
            base(name)
        {
            _context = context;
        }
    }
}
