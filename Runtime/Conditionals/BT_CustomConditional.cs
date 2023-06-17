using System;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_AConditional"/> with custom delegate support
    /// </summary>
    public sealed class BT_CustomConditional : BT_AConditional
    {
        private Func<bool> _canExecute;

        public BT_CustomConditional(string name = "Custom") :
            base(name)
        {
        }

        public BT_CustomConditional WithCanExecute(Func<bool> value)
        {
            _canExecute = value;
            return this;
        }

        public override bool CanExecute()
        {
            return _canExecute();
        }
    }
}
