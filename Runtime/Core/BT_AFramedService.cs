using UnityEngine;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_AService"/> which executes after certain amount of frames passes
    /// </summary>
    public abstract class BT_AFramedService : BT_AService
    {
        protected readonly int _delay;

        protected int _framestamp = 0;

        public BT_AFramedService(int delay = 15, string name = "Framed") :
            base(name)
        {
            _delay = delay;
        }

        private int Nowstamp
        {
            get => Time.frameCount;
        }

        public override void Execute()
        {
            var nowstamp = Nowstamp;
            if (_framestamp > nowstamp)
                return;

            _framestamp = nowstamp + _delay;
            base.Execute();
        }
    }

    /// <summary>
    /// <see cref="BT_AFramedService"/> implementation with build-in context <see cref="T"/> support of any type
    /// </summary>
    public abstract class BT_AFramedService<T> : BT_AFramedService
    {
        protected readonly T _context;

        public BT_AFramedService(T context, int delay = 15, string name = "Framed") :
            base(delay, name)
        {
            _context = context;
        }
    }
}
