namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ANode"/> which executes its child tasks in sequence until one succeed or all fail
    /// </summary>
    public sealed class BT_SelectorNode : BT_ANode
    {
        public BT_SelectorNode(string name = "") :
            base(name)
        {
        }
        
        protected override BT_EStatus OnUpdate()
        {
            for (; _current < _tasks.Length; ++_current)
            {
                var result = _tasks[_current].Update();

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
