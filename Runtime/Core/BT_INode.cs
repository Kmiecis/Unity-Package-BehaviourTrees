using System.Collections.Generic;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// Behaviour Tree node interface
    /// </summary>
    public interface BT_INode : BT_ITask
    {
        /// <summary>
        /// Returns all node children
        /// </summary>
        IEnumerable<BT_ITask> GetChildren();
    }
}