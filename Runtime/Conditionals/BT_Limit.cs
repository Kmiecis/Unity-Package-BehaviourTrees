using System;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_AConditional"/> which halts a task execution after a certain amount of time passes
    /// </summary>
    public sealed class BT_Limit : BT_AConditional
    {
        private readonly long _limit;
        private readonly long _deviation;
        private readonly Random _random;

        private long _timestamp;

        public BT_Limit(float limit, float deviation = 0.0f, Random random = null) :
            base("Limit")
        {
            _limit = UTime.ToTicks(limit);
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
            Remaining = _limit + _random.NextLong(-_deviation, +_deviation);
        }

        public override bool CanExecute()
        {
            return Remaining > 0L;
        }

        public override string ToString()
        {
            var remaining = Math.Max(UTime.ToSeconds(Remaining), 0.0f).ToString("F1");
            return base.ToString() + " [" + remaining + ']';
        }
    }
}
