using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.BehaviorTree
{
    public abstract class CompositeNode : BehaviorTreeNode
    {
        protected readonly List<BehaviorTreeNode> _children = new();

        public CompositeNode(List<BehaviorTreeNode> children)
        {
            _children = children;
        }
    }
}
