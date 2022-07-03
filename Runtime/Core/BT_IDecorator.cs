namespace Common.BehaviourTrees
{
    /// <summary>
    /// Behaviour Tree decorator interface to manage task execution result
    /// </summary>
    public interface BT_IDecorator
    {
        /// <summary>
        /// Called once before task execution
        /// </summary>
        void Start();

        /// <summary>
        /// Manipulates a task execution result
        /// </summary>
        BT_EStatus Decorate(BT_EStatus status);

        /// <summary>
        /// Called once after task execution finishes
        /// </summary>
        void Finish(BT_EStatus status);
    }
}
