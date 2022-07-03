using System;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_AConditional"/> with custom delegate support
    /// </summary>
    public sealed class BT_DelegateConditional : BT_AConditional
    {
        private Func<bool> _canExecute;

        public BT_DelegateConditional(string name = "Delegate") :
            base(name)
        {
        }

        public Func<bool> CanExecuteAction
        {
            set => _canExecute = value;
        }

        public override bool CanExecute()
        {
            if (_canExecute != null)
            {
                return _canExecute();
            }
            return true;
        }
    }
}
