using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// <see cref="BT_ATask"/> node with a multiple child tasks support
    /// </summary>
    [Serializable]
    public abstract class BT_ANode : BT_ATask, BT_INode
    {
        [Tooltip("Children of this node.\nA child is executed in order determined by the node.")]
        [SerializeReference] protected List<BT_ITask> _children;

        protected int _current;

        public BT_ITask Current
            => _children[_current];

        public BT_ANode(string name = null) :
            base(name)
        {
            _children = new List<BT_ITask>();
        }

        public BT_ANode AddChildren(params BT_ITask[] children)
        {
            _children.AddRange(children);
            return this;
        }

        public IEnumerable<BT_ITask> GetChildren()
        {
            return _children;
        }

        protected override void OnStart()
        {
            _current = 0;
        }

        protected override void OnFinish()
        {
            AbortChildren();
        }

        protected void AbortChildren()
        {
            foreach (var child in _children)
            {
                child.Abort();
            }
        }
    }
}
