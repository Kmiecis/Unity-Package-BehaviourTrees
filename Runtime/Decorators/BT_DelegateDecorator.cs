using System;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ADecorator"/> with custom delegate support
    /// </summary>
    public sealed class BT_DelegateDecorator : BT_ADecorator
    {
        private Func<BT_EStatus, BT_EStatus> _decorate;

        public BT_DelegateDecorator(string name = "Delegate") :
            base(name)
        {
        }

        public Func<BT_EStatus, BT_EStatus> DecorateAction
        {
            set => _decorate = value;
        }

        public override BT_EStatus Decorate(BT_EStatus status)
        {
            if (_decorate != null)
            {
                return _decorate(status);
            }
            return status;
        }
    }
}
