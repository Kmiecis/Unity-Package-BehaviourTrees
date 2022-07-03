using System;
using UnityEngine;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ADecorator"/> which repeats a task for a certain amount of frames
    /// </summary>
    public sealed class BT_RepeatForFrames : BT_ADecorator
    {
        private readonly int _duration;

        private int _framestamp;

        public BT_RepeatForFrames(int duration) :
            base("RepeatForFrames")
        {
            _duration = duration;
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
            base.OnStart();

            Remaining = _duration;
        }

        public override BT_EStatus Decorate(BT_EStatus status)
        {
            if (status == BT_EStatus.Success)
            {
                if (Remaining > 0)
                {
                    return BT_EStatus.Running;
                }
                return BT_EStatus.Success;
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
