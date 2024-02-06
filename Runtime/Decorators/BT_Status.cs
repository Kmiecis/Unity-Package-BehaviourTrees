using System;
using UnityEngine;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ADecorator"/> which overrides a task execution result
    /// </summary>
    [Serializable]
    [BT_ItemMenu("Status", BT_MenuPath.Core, BT_MenuGroup.Core)]
    public sealed class BT_Status : BT_ADecorator
    {
        [SerializeField] private BT_EStatus _status;

        public BT_Status() :
            this(BT_EStatus.Running)
        {
        }

        public BT_Status(BT_EStatus status) :
            base("Status")
        {
            _status = status;
        }

        public override BT_EStatus Decorate(BT_EStatus status)
        {
            return _status;
        }
    }
}
