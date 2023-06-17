using System;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_AService"/> with custom delegate support
    /// </summary>
    public class BT_CustomService : BT_AService
    {
        private Action _onUpdate;

        public BT_CustomService(string name = "Custom") :
            base(name)
        {
        }

        public BT_CustomService WithOnUpdate(Action value)
        {
            _onUpdate = value;
            return this;
        }

        protected override void OnUpdate()
        {
            _onUpdate();
        }
    }
}
