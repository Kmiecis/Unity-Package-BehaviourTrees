namespace Common.BehaviourTrees
{
    /// <summary>
    /// Behaviour Tree conditional interface to manage task execution availability
    /// </summary>
    public interface BT_IConditional
    {
        /// <summary>
        /// Called once before task execution
        /// </summary>
        void Start();

        /// <summary>
        /// Declares whether a task should be executed
        /// </summary>
        bool CanExecute();

        /// <summary>
        /// Called once after task execution finishes
        /// </summary>
        void Finish(BT_EStatus status);
    }
}
