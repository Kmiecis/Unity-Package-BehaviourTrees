using System;
using UnityEngine;
using Random = System.Random;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ADecorator"/> which repeats a task for a certain amount of frames
    /// </summary>
    public sealed class BT_RepeatForFrames : BT_ADecorator
    {
        private readonly int _duration;
        private readonly int _deviation;
        private readonly Random _random;

        private bool _repeating;
        private int _framestamp;

        public BT_RepeatForFrames(int duration, int deviation = 0, Random random = null) :
            base("RepeatForFrames")
        {
            _duration = duration;
            _deviation = deviation;
            _random = random ?? new Random();
        }

        private int Nowstamp
        {
            get => Time.frameCount;
        }

        public int Remaining
        {
            get => _framestamp - Nowstamp;
            set => _framestamp = Nowstamp + value;
        }

        protected override void OnStart()
        {
            if (!_repeating)
            {
                _repeating = true;

                Remaining = _duration + _random.Next(-_deviation, +_deviation);
            }
        }

        public override BT_EStatus Decorate(BT_EStatus status)
        {
            if (status != BT_EStatus.Running)
            {
                if (Remaining > 0)
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
            var remaining = Math.Max(Remaining, 0).ToString();
            return base.ToString() + " [" + remaining + ']';
        }
    }
}
