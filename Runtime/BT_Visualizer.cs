using UnityEngine;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="MonoBehaviour"/> which provides visual representation of the tasks in the inspector
    /// </summary>
    public class BT_Visualizer : MonoBehaviour
    {
        private BT_ITask[] _tasks;

        public BT_ITask[] Tasks
        {
            get => _tasks;
            set => _tasks = value;
        }

        public BT_ITask Task
        {
            get => _tasks[0];
            set => _tasks = new BT_ITask[] { value };
        }
    }
}
