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

        protected float _timestamp;

        public BT_ATimedService(float delay = 0.1f, string name = "Timed") :
            base(name)
        {
            _delay = delay;
        }

        public override void Update()
        {
            var nowstamp = BT_Time.Timestamp;
            if (_timestamp > nowstamp)
            {
                return;
            }

            _timestamp = nowstamp + _delay;
            OnUpdate();
        }
    }
}
