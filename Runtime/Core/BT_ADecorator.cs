using System;
using UnityEngine;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// Base <see cref="BT_IDecorator"/> implementation
    /// </summary>
    [Serializable]
    public abstract class BT_ADecorator : BT_IDecorator
    {
        [HideInInspector] [SerializeField] protected string _name;

        protected bool _started;

        public string Name
            => _name;

        public BT_ADecorator(string name = null)
        {
            _name = name ?? GetType().Name;
        }

        public void Start()
        {
            if (!_started)
            {
                OnStart();
                _started = true;
            }
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

        protected virtual void OnStart()
        {
        }

        protected virtual void OnFinish(BT_EStatus status)
        {
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
