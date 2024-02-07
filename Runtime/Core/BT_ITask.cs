namespace Common.BehaviourTrees
{
    /// <summary>
    /// Behaviour Tree task interface
    /// </summary>
    public interface BT_ITask
    {
        /// <summary>
        /// Called every time a task is updated
        /// </summary>
        BT_EStatus Update();

        /// <summary>
        /// Aborts a task at arbitrary point in execution flow
        /// </summary>
        void Abort();
    }
}
