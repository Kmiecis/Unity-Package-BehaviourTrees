using System.Linq;
using UnityEngine;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// Behaviour Tree blackboard script that allows a whole tree to be created within inspector
    /// </summary>
    [AddComponentMenu(nameof(Common) + "/" + nameof(BehaviourTrees) + "/" + nameof(BT_Blackboard))]
    public class BT_Blackboard : MonoBehaviour
    {
        [SerializeField] private BT_RootNode _root;

        public BT_Blackboard()
        {
            _root = new BT_RootNode();
        }

        private void Update()
        {
            _root.Update();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            enabled = _root.GetChildren().Count() > 0;
        }
#endif
    }
}