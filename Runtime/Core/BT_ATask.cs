using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// Base <see cref="BT_ITask"/> implementation
    /// </summary>
    [Serializable]
    public abstract class BT_ATask : BT_ITask
    {
        [HideInInspector] [SerializeField] protected string _name;

        [Tooltip("Conditionals affect the task execution availability.\nA task will only be executed once all conditions pass.")]
        [Unfolded] [SerializeReference] protected List<BT_IConditional> _conditionals;
        [Tooltip("Decorators alter the task result status.\nA task will have its status ran through all decorators.")]
        [Unfolded] [SerializeReference] protected List<BT_IDecorator> _decorators;
        [Tooltip("Services run along with the task.\nA task will not be affected directly by any of the services.")]
        [Unfolded] [SerializeReference] protected List<BT_IService> _services;

        [HideInInspector] [SerializeField] protected BT_EStatus _status;
#if UNITY_EDITOR
        [HideInInspector] [SerializeField] private float _updated;
#endif

        public string Name
            => _name;

        public BT_EStatus Status
            => _status;

        public BT_ATask(string name = null)
        {
            _name = name ?? GetType().Name;

            _conditionals = new List<BT_IConditional>();
            _decorators = new List<BT_IDecorator>();
            _services = new List<BT_IService>();
        }

        public BT_ATask AddConditionals(params BT_IConditional[] conditionals)
        {
            _conditionals.AddRange(conditionals);
            return this;
        }

        public BT_ATask AddDecorators(params BT_IDecorator[] decorators)
        {
            _decorators.AddRange(decorators);
            return this;
        }

        public BT_ATask AddServices(params BT_IService[] services)
        {
            _services.AddRange(services);
            return this;
        }

        public BT_EStatus Update()
        {
#if UNITY_EDITOR
            _updated = BT_Time.TimestampUnscaled;
#endif
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
                Finish();

                _status = BT_EStatus.Failure;
            }
        }

        #region Conditionals
        protected void StartConditionals()
        {
            foreach (var conditional in _conditionals)
            {
                conditional.Start();
            }
        }

        protected bool CanExecute()
        {
            foreach (var conditional in _conditionals)
            {
                if (!conditional.CanExecute())
                {
                    return false;
                }
            }
            return true;
        }

        protected void FinishConditionals(BT_EStatus status)
        {
            foreach (var conditional in _conditionals)
            {
                conditional.Finish(status);
            }
        }
        #endregion

        #region Decorators
        protected void StartDecorators()
        {
            foreach (var decorator in _decorators)
            {
                decorator.Start();
            }
        }

        protected BT_EStatus Decorate(BT_EStatus status)
        {
            foreach (var decorator in _decorators)
            {
                status = decorator.Decorate(status);
            }
            return status;
        }

        protected void FinishDecorators(BT_EStatus status)
        {
            foreach (var decorator in _decorators)
            {
                decorator.Finish(status);
            }
        }
        #endregion

        #region Services
        protected void ExecuteServices()
        {
            foreach (var service in _services)
            {
                service.Update();
            }
        }
        #endregion

        public override string ToString()
        {
            return _name;
        }
    }
}
