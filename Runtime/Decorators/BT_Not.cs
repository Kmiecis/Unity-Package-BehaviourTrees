using System;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ADecorator"/> which negates a task execution result
    /// </summary>
    [Serializable]
    [BT_Menu("Not", BT_MenuPath.Core, BT_MenuGroup.Core)]
    public sealed class BT_Not : BT_ADecorator
    {
        public BT_Not() :
            base("Not")
        {
        }

        public override BT_EStatus Decorate(BT_EStatus status)
        {
            switch (status)
            {
                case BT_EStatus.Failure:
                    return BT_EStatus.Success;

                case BT_EStatus.Success:
                    return BT_EStatus.Failure;
            }

            return status;
        }
    }
}
