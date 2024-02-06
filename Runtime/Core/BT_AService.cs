using System;
using UnityEngine;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// Base <see cref="BT_IService"/> implementation
    /// </summary>
    [Serializable]
    public abstract class BT_AService : BT_IService
    {
        [HideInInspector] [SerializeField] protected string _name;

        public string Name
            => _name;

        public BT_AService(string name = null)
        {
            _name = name ?? GetType().Name;
        }

        public virtual void Update()
        {
            OnUpdate();
        }

        protected abstract void OnUpdate();
        
        public override string ToString()
        {
            return _name;
        }
    }
}
