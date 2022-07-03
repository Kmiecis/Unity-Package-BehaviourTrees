namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ADecorator"/> which overrides a task execution result
    /// </summary>
    public sealed class BT_Status : BT_ADecorator
    {
        private readonly BT_EStatus _status;

        public BT_Status(BT_EStatus status) :
            base(status.ToString())
        {
            _status = status;
        }

        public override BT_EStatus Decorate(BT_EStatus status)
        {
            return _status;
        }
    }
}
