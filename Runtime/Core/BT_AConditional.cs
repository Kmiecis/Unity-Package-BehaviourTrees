using System;
using UnityEngine;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// Base <see cref="BT_IConditional"/> implementation
    /// </summary>
    [Serializable]
    public abstract class BT_AConditional : BT_IConditional
    {
        [HideInInspector] [SerializeField] protected string _name;
        
        protected bool _started;

        public string Name
            => _name;

        public BT_AConditional(string name = null)
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

        public abstract bool CanExecute();

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
