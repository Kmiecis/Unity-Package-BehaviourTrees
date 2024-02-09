using System;
using UnityEngine;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_AService"/> which sends a message using <see cref="Debug.LogError"/>
    /// </summary>
    [Serializable]
    [BT_Menu("Error", BT_MenuPath.Debug, BT_MenuGroup.Debug)]
    public sealed class BT_ErrorService : BT_AService
    {
        [SerializeField] private string _message;

        public BT_ErrorService() :
            this(null)
        {
        }

        public BT_ErrorService(string message) :
            base("Error")
        {
            _message = message;
        }

        protected override void OnUpdate()
        {
            Debug.LogError(_message);
        }
    }
}