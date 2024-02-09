using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ATask"/> which executes for a certain amount of time
    /// </summary>
    [Serializable]
    [BT_Menu("Wait", BT_MenuPath.Task, BT_MenuGroup.Core)]
    public sealed class BT_Wait : BT_ATask
    {
        [SerializeField] private float _duration;
        [SerializeField] private float _deviation;

        [SerializeField] [ReadOnly] private float _remaining;

        private float _timestamp;

        public BT_Wait() :
            this(0.0f, 0.0f)
        {
        }

        public BT_Wait(float duration, float deviation = 0.0f) :
            base("Wait")
        {
            _duration = duration;
            _deviation = deviation;
        }

        protected override void OnStart()
        {
            _remaining = _duration + Random.Range(-_deviation, +_deviation);
            _timestamp = BT_Time.Timestamp;
        }

        protected override BT_EStatus OnUpdate()
        {
            _remaining -= BT_Time.GetDeltaTime(ref _timestamp);
            if (_remaining > 0.0f)
            {
                return BT_EStatus.Running;
            }
            return BT_EStatus.Success;
        }

        protected override void OnFinish()
        {
            _remaining = 0.0f;
        }
    }
}
