using System;
using UnityEngine;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_AService"/> which executes after certain amount of time passes
    /// </summary>
    [Serializable]
    public abstract class BT_ATimedService : BT_AService
    {
        [SerializeField] protected float _delay;

        protected float _remaining;

        public BT_ATimedService(float delay = 0.1f, string name = "Timed") :
            base(name)
        {
            _delay = delay;
        }

        public override void Update()
        {
            _remaining -= Time.deltaTime;
            if (_remaining > 0.0f)
            {
                return;
            }

            _remaining = _delay;
            OnUpdate();
        }
    }
}
