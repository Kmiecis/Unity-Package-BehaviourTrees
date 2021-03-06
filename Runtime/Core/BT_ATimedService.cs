using UnityEngine;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_AService"/> which executes after certain amount of time passes
    /// </summary>
    public abstract class BT_ATimedService : BT_AService
    {
        protected readonly float _delay;

        protected float _timestamp = 0.0f;

        public BT_ATimedService(float delay = 0.25f, string name = "Timed") :
            base(name)
        {
            _delay = delay;
        }

        private float Nowstamp
        {
            get => Time.time;
        }

        public override void Execute()
        {
            var nowstamp = Nowstamp;
            if (_timestamp > nowstamp)
                return;

            _timestamp = nowstamp + _delay;
            base.Execute();
        }
    }

    /// <summary>
    /// <see cref="BT_AService"/> implementation with build-in context <see cref="T"/> support of any type
    /// </summary>
    public abstract class BT_ATimedService<T> : BT_ATimedService
    {
        protected readonly T _context;

        public BT_ATimedService(T context, float delay = 0.25f, string name = "Timed") :
            base(delay, name)
        {
            _context = context;
        }
    }
}
