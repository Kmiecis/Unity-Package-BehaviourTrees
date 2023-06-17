using System;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ADecorator"/> with custom delegate support
    /// </summary>
    public sealed class BT_CustomDecorator : BT_ADecorator
    {
        private Func<BT_EStatus, BT_EStatus> _decorate;

        public BT_CustomDecorator(string name = "Custom") :
            base(name)
        {
        }

        public BT_CustomDecorator WithDecorate(Func<BT_EStatus, BT_EStatus> value)
        {
            _decorate = value;
            return this;
        }

        public override BT_EStatus Decorate(BT_EStatus status)
        {
            return _decorate(status);
        }
    }
}
