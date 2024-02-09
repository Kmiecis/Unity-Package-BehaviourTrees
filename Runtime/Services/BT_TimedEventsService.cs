using System;
using UnityEngine;
using UnityEngine.Events;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ATimedService"/> with <see cref="UnityEvent"/> support
    /// </summary>
    [Serializable]
    [BT_Menu("Timed Events", BT_MenuPath.Core, BT_MenuGroup.Core)]
    public sealed class BT_TimedEventsService : BT_ATimedService
    {
        [SerializeField] private UnityEvent _events;

        public BT_TimedEventsService(float delay = 0.1f) :
            base(delay, "Timed Events")
        {
        }

        protected override void OnUpdate()
        {
            if (_events != null)
                _events.Invoke();
        }
    }
}