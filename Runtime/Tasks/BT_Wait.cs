using System;
using UnityEngine;
using Random = System.Random;

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

        private float _timestamp = 0.0f;

        public BT_Wait(float duration, float deviation = 0.0f, Random random = null) :
            base("Wait")
        {
            _duration = duration;
            _deviation = deviation;
            _random = random ?? new Random();
        }

        private float Nowstamp
        {
            get => Time.time;
        }

        public float Remaining
        {
            get => _timestamp - Nowstamp;
            set => _timestamp = Nowstamp + value;
        }
        
        protected override void OnStart()
        {
            base.OnStart();

            Remaining = _duration + _random.NextFloat(-_deviation, +_deviation);
        }

        protected override BT_EStatus OnExecute()
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
