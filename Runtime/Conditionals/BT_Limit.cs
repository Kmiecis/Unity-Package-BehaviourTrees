using System;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_AConditional"/> which halts a task execution after a certain amount of time passes
    /// </summary>
    public sealed class BT_Limit : BT_AConditional
    {
        private readonly float _limit;
        private readonly float _deviation;
        private readonly Random _random;

        private float _timestamp;

        public BT_Limit(float limit, float deviation = 0.0f, Random random = null) :
            base("Limit")
        {
            _limit = limit;
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
            Remaining = _limit + _random.NextFloat(-_deviation, +_deviation);
        }

        public override bool CanExecute()
        {
            return Remaining > 0.0f;
        }

        public override string ToString()
        {
            var remaining = Math.Max(Remaining, 0.0f).ToString("F1");
            return base.ToString() + " [" + remaining + ']';
        }
    }
}
