using System;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_AConditional"/> which prevents a task execution until a certain amount of time passes
    /// </summary>
    public sealed class BT_Cooldown : BT_AConditional
    {
        private readonly float _cooldown;
        private readonly float _deviation;
        private readonly Random _random;

        private float _timestamp;

        public BT_Cooldown(float cooldown, float deviation = 0.0f, Random random = null) :
            base("Cooldown")
        {
            _cooldown = cooldown;
            _deviation = deviation;
            _random = random ?? new Random();
        }

        public float Remaining
        {
            get => _timestamp - UTime.UtcNow;
            set => _timestamp = UTime.UtcNow + value;
        }

        public override bool CanExecute()
        {
            return Remaining <= 0.0f;
        }

        protected override void OnFinish(BT_EStatus result)
        {
            Remaining = _cooldown + _random.NextFloat(-_deviation, +_deviation);
        }

        public override string ToString()
        {
            var remaining = Math.Max(Remaining, 0.0f).ToString("F1");
            return base.ToString() + " [" + remaining + ']';
        }
    }
}
