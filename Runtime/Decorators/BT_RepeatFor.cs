using System;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ADecorator"/> which repeats a task for a certain amount of time
    /// </summary>
    public sealed class BT_RepeatFor : BT_ADecorator
    {
        private readonly long _duration;
        private readonly long _deviation;
        private readonly Random _random;

        private bool _repeating;
        private long _timestamp;

        public BT_RepeatFor(float duration, float deviation = 0.0f, Random random = null) :
            base("RepeatFor")
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
            if (!_repeating)
            {
                _repeating = true;

                Remaining = _duration + _random.NextLong(-_deviation, +_deviation);
            }
        }

        public override BT_EStatus Decorate(BT_EStatus status)
        {
            if (status != BT_EStatus.Running)
            {
                if (Remaining > 0L)
                {
                    return BT_EStatus.Running;
                }

                _repeating = false;
                return status;
            }

            return status;
        }

        public override string ToString()
        {
            var remaining = Math.Max(UTime.ToSeconds(Remaining), 0.0f).ToString("F1");
            return base.ToString() + " [" + remaining + ']';
        }
    }
}
