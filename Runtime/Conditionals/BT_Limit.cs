using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_AConditional"/> which halts a task execution after a certain amount of time passes
    /// </summary>
    [Serializable]
    [BT_ItemMenu("Limit", BT_MenuPath.Core, BT_MenuGroup.Core)]
    public sealed class BT_Limit : BT_AConditional
    {
        [SerializeField] private float _limit;
        [SerializeField] private float _deviation;

        [SerializeField] [ReadOnly] private float _remaining;

        private float _timestamp;

        public BT_Limit() :
            this(0.0f, 0.0f)
        {
        }

        public BT_Limit(float limit, float deviation = 0.0f) :
            base("Limit")
        {
            _limit = limit;
            _deviation = deviation;
        }

        protected override void OnStart()
        {
            _remaining = _limit + Random.Range(-_deviation, +_deviation);
            _timestamp = BT_Time.Nowstamp;
        }

        public override bool CanExecute()
        {
            _remaining -= BT_Time.GetDeltaTime(ref _timestamp);
            return _remaining > 0.0f;
        }
    }
}
