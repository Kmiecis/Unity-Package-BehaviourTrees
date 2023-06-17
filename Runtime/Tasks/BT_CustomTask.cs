using System;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ATask"/> with custom delegate support
    /// </summary>
    public sealed class BT_CustomTask : BT_ATask
    {
        private Action _onStart;
        private Func<BT_EStatus> _onUpdate;
        private Action _onFinish;

        public BT_CustomTask(string name = "Custom") :
            base(name)
        {
        }

        public BT_CustomTask WithOnStart(Action value)
        {
            _onStart = value;
            return this;
        }

        public BT_CustomTask WithOnUpdate(Func<BT_EStatus> value)
        {
            _onUpdate = value;
            return this;
        }

        public BT_CustomTask WithOnFinish(Action value)
        {
            _onFinish = value;
            return this;
        }

        protected override void OnStart()
        {
            if (_onStart != null)
            {
                _onStart();
            }
        }

        protected override BT_EStatus OnUpdate()
        {
            if (_onUpdate != null)
            {
                return _onUpdate();
            }
            return BT_EStatus.Success;
        }

        protected override void OnFinish()
        {
            if (_onFinish != null)
            {
                _onFinish();
            }
        }
    }
}
