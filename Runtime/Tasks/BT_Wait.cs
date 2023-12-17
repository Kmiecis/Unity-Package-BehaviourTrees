using System;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ATask"/> which executes for a certain amount of time
    /// </summary>
    public sealed class BT_Wait : BT_ATask
    {
        private readonly float _duration;
        private readonly float _deviation;
        private readonly Random _random;

        private float _timestamp;

        public BT_Wait(float duration, float deviation = 0.0f, Random random = null) :
            base("Wait")
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
            Remaining = _duration + _random.NextFloat(-_deviation, +_deviation);
        }

        protected override BT_EStatus OnUpdate()
        {
            if (Remaining > 0.0f)
            {
                return BT_EStatus.Running;
            }
            return BT_EStatus.Success;
        }

        public override string ToString()
        {
            var remaining = Math.Max(Remaining, 0.0f).ToString("F1");
            return base.ToString() + " [" + remaining + ']';
        }
    }
}
