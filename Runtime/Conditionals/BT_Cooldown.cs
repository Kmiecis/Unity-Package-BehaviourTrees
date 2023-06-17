using System;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_AConditional"/> which prevents a task execution until a certain amount of time passes
    /// </summary>
    public sealed class BT_Cooldown : BT_AConditional
    {
        private readonly long _cooldown;
        private readonly long _deviation;
        private readonly Random _random;

        private long _timestamp;

        public BT_Cooldown(float cooldown, float deviation = 0.0f, Random random = null) :
            base("Cooldown")
        {
            _cooldown = UTime.ToTicks(cooldown);
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

        public override bool CanExecute()
        {
            return Remaining <= 0L;
        }

        protected override void OnFinish(BT_EStatus result)
        {
            Remaining = _cooldown + _random.NextLong(-_deviation, +_deviation);
        }

        public override string ToString()
        {
            var remaining = Math.Max(UTime.ToSeconds(Remaining), 0.0f).ToString("F1");
            return base.ToString() + " [" + remaining + ']';
        }
    }
}
