using System;
using UnityEngine;
using Random = System.Random;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_AConditional"/> which prevents a task execution until a certain amount of frames passes
    /// </summary>
    public sealed class BT_CooldownFrames : BT_AConditional
    {
        private readonly int _cooldown;
        private readonly int _deviation;
        private readonly Random _random;

        private int _framestamp = 0;

        public BT_CooldownFrames(int cooldown, int deviation = 0, Random random = null) :
            base("Cooldown")
        {
            _cooldown = cooldown;
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

        public override bool CanExecute()
        {
            return Remaining <= 0;
        }

        protected override void OnFinish(BT_EStatus result)
        {
            base.OnFinish(result);

            Remaining = _cooldown + _random.Next(-_deviation, +_deviation);
        }

        public override string ToString()
        {
            var remaining = Math.Max(Remaining, 0).ToString();
            return base.ToString() + " [" + remaining + ']';
        }
    }
}
