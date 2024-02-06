using System;
using UnityEngine;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ADecorator"/> which repeats a task for a certain number of times
    /// </summary>
    [Serializable]
    [BT_ItemMenu("Repeat", BT_MenuPath.Core, BT_MenuGroup.Core)]
    public sealed class BT_Repeat : BT_ADecorator
    {
        private const int RepeatInfinitely = -1;

        [SerializeField] private int _repeats;

        [SerializeField] [ReadOnly] private int _remaining;

        public BT_Repeat() :
            this(RepeatInfinitely)
        {
        }

        public BT_Repeat(int repeats = RepeatInfinitely) :
            base("Repeat")
        {
            _repeats = repeats;
        }

        public int Remaining
        {
            get => _remaining;
        }

        protected override void OnStart()
        {
            if (_remaining == 0)
            {
                _remaining = _repeats;
            }
            _remaining -= 1;
        }

        public override BT_EStatus Decorate(BT_EStatus status)
        {
            if (status != BT_EStatus.Running)
            {
                if (_remaining != 0)
                {
                    return BT_EStatus.Running;
                }

                return status;
            }

            return status;
        }
    }
}
