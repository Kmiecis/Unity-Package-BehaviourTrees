using Common.BehaviourTrees;
using UnityEditor;

namespace CommonEditor.BehaviourTrees
{
    [CustomPropertyDrawer(typeof(BT_ANode), true)]
    public class BT_ANodeDrawer : BT_ATaskDrawer
    {
        private static readonly string[] NodeDrawOptions = new string[] { "_conditionals", "_decorators", "_services", "_children" };
        private static readonly string[] ForcedDrawOptions = new string[] { "_children" };

        protected override string[] GetDrawOptions()
        {
            return NodeDrawOptions;
        }

        protected override string[] GetForcedDrawOptions()
        {
            return ForcedDrawOptions;
        }
    }
}