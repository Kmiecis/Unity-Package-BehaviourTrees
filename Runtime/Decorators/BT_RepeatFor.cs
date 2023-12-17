using System;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ADecorator"/> which repeats a task for a certain amount of time
    /// </summary>
    public sealed class BT_RepeatFor : BT_ADecorator
    {
        private readonly float _duration;
        private readonly float _deviation;
        private readonly Random _random;

        private bool _repeating;
        private float _timestamp;

        public BT_RepeatFor(float duration, float deviation = 0.0f, Random random = null) :
            base("RepeatFor")
        {
            _duration = duration;
            _deviation = deviation;
            _random = random ?? new Random();
        }

        public float Remaining
        {
            get => _timestamp - UTime.UtcNow;
            set => _timestamp = UTime.UtcNow + value;
        }

        protected override void OnStart()
        {
            if (!_repeating)
            {
                _repeating = true;

                Remaining = _duration + _random.NextFloat(-_deviation, +_deviation);
            }
        }

        public override BT_EStatus Decorate(BT_EStatus status)
        {
            if (status != BT_EStatus.Running)
            {
                if (Remaining > 0.0f)
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
            var remaining = Math.Max(Remaining, 0.0f).ToString("F1");
            return base.ToString() + " [" + remaining + ']';
        }
    }
}
