using System;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ATask"/> which executes for a certain amount of time
    /// </summary>
    public sealed class BT_Wait : BT_ATask
    {
        private readonly long _duration;
        private readonly long _deviation;
        private readonly Random _random;

        private long _timestamp;

        public BT_Wait(float duration, float deviation = 0.0f, Random random = null) :
            base("Wait")
        {
            _duration = UTime.ToTicks(duration);
            _deviation = UTime.ToTicks(deviation);
            _random = random ?? new Random();
        }

        private long Nowstamp
        {
            get => UTime.Now;
        }

        public long Remaining
        {
            get => _timestamp - Nowstamp;
            set => _timestamp = Nowstamp + value;
        }
        
        protected override void OnStart()
        {
            Remaining = _duration + _random.NextLong(-_deviation, +_deviation);
        }

        protected override BT_EStatus OnUpdate()
        {
            if (Remaining > 0L)
            {
                return BT_EStatus.Running;
            }
            return BT_EStatus.Success;
        }

        public override string ToString()
        {
            var remaining = Math.Max(UTime.ToSeconds(Remaining), 0.0f).ToString("F1");
            return base.ToString() + " [" + remaining + ']';
        }
    }
}
