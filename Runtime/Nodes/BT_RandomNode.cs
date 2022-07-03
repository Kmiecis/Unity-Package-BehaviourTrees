using System;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_AMultiNode"/> which executes a random child task as long as it is running
    /// </summary>
    public sealed class BT_RandomNode : BT_AMultiNode
    {
        private readonly Random _random;

        public BT_RandomNode(string name = "", Random random = null) :
            base(name)
        {
            _random = random ?? new Random();
        }
        
        protected override void OnStart()
        {
            base.OnStart();

            _current = _random.Next(0, _tasks.Length);
        }

        protected override BT_EStatus OnUpdate()
        {
            var current = CurrentTask;
            return current.Execute();
        }
    }
}
