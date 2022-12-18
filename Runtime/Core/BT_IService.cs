namespace Common.BehaviourTrees
{
    /// <summary>
    /// Behaviour Tree service interface to execute at certain intervals while a task is executed
    /// </summary>
    public interface BT_IService
    {
        /// <summary>
        /// Called every time a task is executed
        /// </summary>
        void Update();
    }
}
