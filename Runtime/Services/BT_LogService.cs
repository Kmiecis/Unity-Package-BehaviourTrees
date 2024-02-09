using System;
using UnityEngine;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_AService"/> which sends a message using <see cref="Debug.Log"/>
    /// </summary>
    [Serializable]
    [BT_Menu("Log", BT_MenuPath.Debug, BT_MenuGroup.Debug)]
    public sealed class BT_LogService : BT_AService
    {
        [SerializeField] private string _message;

        public BT_LogService() :
            this(null)
        {
        }

        public BT_LogService(string message) :
            base("Log")
        {
            _message = message;
        }

        protected override void OnUpdate()
        {
            Debug.Log(_message);
        }
    }
}