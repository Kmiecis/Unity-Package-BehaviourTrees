using System;
using UnityEngine;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_AService"/> which pauses the editor using <see cref="Debug.Break"/>
    /// </summary>
    [Serializable]
    [BT_ItemMenu("Break", BT_MenuPath.Debug, BT_MenuGroup.Debug + 3)]
    public sealed class BT_BreakService : BT_AService
    {
        public BT_BreakService() :
            base("Break")
        {
        }

        protected override void OnUpdate()
        {
#if UNITY_EDITOR
            Debug.Break();
#endif
        }
    }
}