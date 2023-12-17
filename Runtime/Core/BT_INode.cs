namespace Common.BehaviourTrees
{
    /// <summary>
    /// Behaviour Tree node interface
    /// </summary>
    public interface BT_INode : BT_ITask
    {
        /// <summary>
        /// Returns all node tasks
        /// </summary>
        BT_ITask[] Tasks { get; }

        /// <summary>
        /// Returns current node task
        /// </summary>
        BT_ITask Current { get; }
    }
}