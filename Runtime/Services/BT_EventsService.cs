using System;
using UnityEngine;
using UnityEngine.Events;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_AService"/> with <see cref="UnityEvent"/> support
    /// </summary>
    [Serializable]
    [BT_ItemMenu("Events", BT_MenuPath.Core, BT_MenuGroup.Core)]
    public sealed class BT_EventsService : BT_AService
    {
        [SerializeField] private UnityEvent _events;

        public BT_EventsService() :
            base("Events")
        {
        }

        protected override void OnUpdate()
        {
            if (_events != null)
                _events.Invoke();
        }
    }
}