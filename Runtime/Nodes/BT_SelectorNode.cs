namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_AMultiNode"/> which executes its child tasks in sequence until one succeed or all fail
    /// </summary>
    public sealed class BT_SelectorNode : BT_AMultiNode
    {
        public BT_SelectorNode(string name = "") :
            base(name)
        {
        }
        
        protected override BT_EStatus OnExecute()
        {
            for (; _current < _tasks.Length; ++_current)
            {
                var result = _tasks[_current].Execute();

                if (result != BT_EStatus.Failure)
                {
                    return result;
                }
            }

            _current -= 1;
            return BT_EStatus.Failure;
        }
    }
}
