namespace Common.BehaviourTrees
{
    /// <summary>
    /// Base <see cref="BT_ITask"/> implementation
    /// </summary>
    public abstract class BT_ATask : BT_ITask
    {
        protected string _name;
        protected BT_EStatus _status;

        protected BT_IConditional[] _conditionals;
        protected BT_IDecorator[] _decorators;
        protected BT_IService[] _services;

        public BT_ATask(string name = null)
        {
            _name = name ?? GetType().Name;
        }

        public string Name
        {
            get => _name;
        }

        public BT_EStatus Status
        {
            get => _status;
        }

        public BT_IConditional[] Conditionals
        {
            get => _conditionals;
            set => _conditionals = value;
        }

        public BT_IConditional Conditional
        {
            get => _conditionals[0];
            set => _conditionals = new BT_IConditional[] { value };
        }

        public BT_IDecorator[] Decorators
        {
            get => _decorators;
            set => _decorators = value;
        }

        public BT_IDecorator Decorator
        {
            get => _decorators[0];
            set => _decorators = new BT_IDecorator[] { value };
        }

        public BT_IService[] Services
        {
            get => _services;
            set => _services = value;
        }

        public BT_IService Service
        {
            get => _services[0];
            set => _services = new BT_IService[] { value };
        }

        public BT_EStatus Update()
        {
            ExecuteServices();

            if (_status != BT_EStatus.Running)
            {
                Start();
            }

            if (!CanExecute())
            {
                return BT_EStatus.Failure;
            }

            _status = OnUpdate();

            var decorated = Decorate(_status);

            if (_status != BT_EStatus.Running)
            {
                Finish();
            }

            return decorated;
        }

        private void Start()
        {
            OnStart();

            StartConditionals();

            StartDecorators();
        }

        private void Finish()
        {
            FinishDecorators(_status);

            FinishConditionals(_status);

            OnFinish();
        }

        protected virtual void OnStart()
        {
        }

        protected abstract BT_EStatus OnUpdate();

        protected virtual void OnFinish()
        {
        }

        public void Abort()
        {
            if (_status == BT_EStatus.Running)
            {
                _status = BT_EStatus.Failure;

                Finish();
            }
        }

        #region CONDITIONALS
        protected void StartConditionals()
        {
            if (_conditionals != null)
            {
                for (int i = 0; i < _conditionals.Length; ++i)
                {
                    _conditionals[i].Start();
                }
            }
        }

        protected bool CanExecute()
        {
            if (_conditionals != null)
            {
                for (int i = 0; i < _conditionals.Length; ++i)
                {
                    if (!_conditionals[i].CanExecute())
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        protected void FinishConditionals(BT_EStatus status)
        {
            if (_conditionals != null)
            {
                for (int i = 0; i < _conditionals.Length; ++i)
                {
                    _conditionals[i].Finish(status);
                }
            }
        }
        #endregion

        #region DECORATORS
        protected void StartDecorators()
        {
            if (_decorators != null)
            {
                for (int i = 0; i < _decorators.Length; ++i)
                {
                    _decorators[i].Start();
                }
            }
        }

        protected BT_EStatus Decorate(BT_EStatus status)
        {
            if (_decorators != null)
            {
                for (int i = 0; i < _decorators.Length; ++i)
                {
                    status = _decorators[i].Decorate(status);
                }
            }
            return status;
        }

        protected void FinishDecorators(BT_EStatus status)
        {
            if (_decorators != null)
            {
                for (int i = 0; i < _decorators.Length; ++i)
                {
                    _decorators[i].Finish(status);
                }
            }
        }
        #endregion

        #region SERVICES
        protected void ExecuteServices()
        {
            if (_services != null)
            {
                for (int i = 0; i < _services.Length; ++i)
                {
                    _services[i].Update();
                }
            }
        }
        #endregion

        public override string ToString()
        {
            return _name;
        }

        public BT_ATask FindTaskByName(string name)
        {
            return FindTaskByName(this, name);
        }

        public static BT_ATask FindTaskByName(BT_ATask root, string name)
        {
            if (root == null)
            {
                return null;
            }

            if (root._name == name)
            {
                return root;
            }

            if (root is BT_ASingleNode single)
            {
                root = single.Task as BT_ATask;
                return FindTaskByName(root, name);
            }

            if (root is BT_AMultiNode multi)
            {
                var tasks = multi.Tasks;
                for (int i = 0; i < tasks.Length; ++i)
                {
                    root = tasks[i] as BT_ATask;
                    var result = FindTaskByName(root, name);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }

        public T FindTaskByType<T>()
            where T : class, BT_ITask
        {
            return FindTaskByType<T>(this);
        }

        public static T FindTaskByType<T>(BT_ITask root)
            where T : class, BT_ITask
        {
            if (root == null)
            {
                return null;
            }

            if (root is T result)
            {
                return result;
            }

            if (root is BT_ASingleNode single)
            {
                root = single.Task;
                return FindTaskByType<T>(root);
            }

            if (root is BT_AMultiNode multi)
            {
                var tasks = multi.Tasks;
                for (int i = 0; i < tasks.Length; ++i)
                {
                    root = tasks[i];
                    result = FindTaskByType<T>(root);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }
    }

    /// <summary>
    /// <see cref="BT_ATask"/> implementation with build-in context <see cref="T"/> support of any type
    /// </summary>
    public abstract class BT_ATask<T> : BT_ATask
    {
        protected readonly T _context;

        public BT_ATask(T context, string name = null) :
            base(name)
        {
            _context = context;
        }
    }
}
