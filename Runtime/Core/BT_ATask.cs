namespace Common.BehaviourTrees
{
    /// <summary>
    /// Base <see cref="BT_ITask"/> implementation
    /// </summary>
    public abstract class BT_ATask : BT_ITask
    {
        protected readonly string _name;
        protected BT_EStatus _status;

        protected BT_IConditional[] _conditionals;
        protected BT_IDecorator[] _decorators;
        protected BT_IService[] _services;

        public string Name
            => _name;

        public BT_EStatus Status
            => _status;

        public BT_IConditional[] Conditionals
            => _conditionals;

        public BT_IDecorator[] Decorators
            => _decorators;

        public BT_IService[] Services
            => _services;

        public BT_ATask(string name = null)
        {
            _name = name ?? GetType().Name;
        }

        public BT_ATask WithConditionals(params BT_IConditional[] values)
        {
            _conditionals = values;
            return this;
        }

        public BT_ATask WithDecorators(params BT_IDecorator[] values)
        {
            _decorators = values;
            return this;
        }

        public BT_ATask WithServices(params BT_IService[] values)
        {
            _services = values;
            return this;
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
