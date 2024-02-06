using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ADecorator"/> which repeats a task for a certain amount of time
    /// </summary>
    [Serializable]
    [BT_ItemMenu("Repeat For", BT_MenuPath.Core, BT_MenuGroup.Core)]
    public sealed class BT_RepeatFor : BT_ADecorator
    {
        [SerializeField] private float _duration;
        [SerializeField] private float _deviation;

        [SerializeField] [ReadOnly] private float _remaining;

        private bool _repeating;
        private float _timestamp;

        public BT_RepeatFor() :
            this(0.0f, 0.0f)
        {
        }

        public BT_RepeatFor(float duration, float deviation = 0.0f) :
            base("Repeat For")
        {
            _duration = duration;
            _deviation = deviation;
        }

        protected override void OnStart()
        {
            if (!_repeating)
            {
                _repeating = true;

                _remaining = _duration + Random.Range(-_deviation, +_deviation);
                _timestamp = BT_Time.Nowstamp;
            }
        }

        public override BT_EStatus Decorate(BT_EStatus status)
        {
            if (status != BT_EStatus.Running)
            {
                _remaining -= BT_Time.GetDeltaTime(ref _timestamp);
                if (_remaining > 0.0f)
                {
                    return BT_EStatus.Running;
                }

                _repeating = false;
                return status;
            }

            return status;
        }
    }
}
