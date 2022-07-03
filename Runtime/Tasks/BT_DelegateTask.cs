using System;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ATask"/> with custom delegate support
    /// </summary>
    public sealed class BT_DelegateTask : BT_ATask
    {
        private Action _onStart;
        private Func<BT_EStatus> _onUpdate;
        private Action<BT_EStatus> _onFinish;

        public BT_DelegateTask(string name = "Delegate") :
            base(name)
        {
        }

        public Action OnStartAction
        {
            set => _onStart = value;
        }

        public Func<BT_EStatus> OnUpdateAction
        {
            set => _onUpdate = value;
        }

        public Action<BT_EStatus> OnFinishAction
        {
            set => _onFinish = value;
        }

        protected override void OnStart()
        {
            base.OnStart();

            _onStart?.Invoke();
        }

        protected override BT_EStatus OnUpdate()
        {
            if (_onUpdate != null)
            {
                return _onUpdate();
            }
            return BT_EStatus.Success;
        }

        protected override void OnFinish(BT_EStatus status)
        {
            base.OnFinish(status);

            _onFinish?.Invoke(status);
        }
    }
}
