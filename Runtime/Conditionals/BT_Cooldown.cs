using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_AConditional"/> which prevents a task execution until a certain amount of time passes
    /// </summary>
    [Serializable]
    [BT_Menu("Cooldown", BT_MenuPath.Core, BT_MenuGroup.Core)]
    public sealed class BT_Cooldown : BT_AConditional
    {
        [SerializeField] private float _cooldown;
        [SerializeField] private float _deviation;

        [SerializeField] [ReadOnly] private float _remaining;

        private float _timestamp;

        public BT_Cooldown() :
            this(0.0f, 0.0f)
        {
        }

        public BT_Cooldown(float cooldown, float deviation = 0.0f) :
            base("Cooldown")
        {
            _cooldown = cooldown;
            _deviation = deviation;
        }

        protected override void OnStart()
        {
            _timestamp = BT_Time.Timestamp;
        }

        public override bool CanExecute()
        {
            _remaining -= BT_Time.GetDeltaTime(ref _timestamp);
            return _remaining <= 0.0f;
        }

        protected override void OnFinish(BT_EStatus result)
        {
            _remaining = _cooldown + Random.Range(-_deviation, +_deviation);
        }
    }
}
