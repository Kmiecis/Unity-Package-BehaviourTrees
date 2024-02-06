using System;
using UnityEngine;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_AService"/> which sends a message using <see cref="Debug.LogWarning"/>
    /// </summary>
    [Serializable]
    [BT_ItemMenu("Warning", BT_MenuPath.Debug, BT_MenuGroup.Debug + 1)]
    public sealed class BT_WarningService : BT_AService
    {
        [SerializeField] private string _message;

        public BT_WarningService() :
            this(null)
        {
        }

        public BT_WarningService(string message) :
            base("Warning")
        {
            _message = message;
        }

        protected override void OnUpdate()
        {
            Debug.LogWarning(_message);
        }
    }
}