namespace Common.BehaviourTrees
{
    /// <summary>
    /// Base <see cref="BT_IService"/> implementation
    /// </summary>
    public abstract class BT_AService : BT_IService
    {
        protected string _name;

        public BT_AService(string name = null)
        {
            _name = name ?? GetType().Name;
        }

        public virtual void Execute()
        {
            OnUpdate();
        }

        protected abstract void OnUpdate();
        
        public override string ToString()
        {
            return _name;
        }
    }

    /// <summary>
    /// <see cref="BT_AService"/> implementation with build-in context <see cref="T"/> support of any type
    /// </summary>
    public abstract class BT_AService<T> : BT_AService
    {
        protected readonly T _context;

        public BT_AService(T context, string name = null) :
            base(name)
        {
            _context = context;
        }
    }
}
