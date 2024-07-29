using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.BehaviorTree
{
    public class Selector : CompositeNode
    {
        public override NODE_STATE Evaluate()
        {
            foreach (BehaviorTreeNode node in _children)
            {
                NODE_STATE nodeState = node.Evaluate();

                switch (nodeState)
                {
                    case NODE_STATE.SUCCESS or NODE_STATE.RUNNING:
                        _nodeState = nodeState;
                        return _nodeState;
                    default:
                        continue;
                }
            }

            _nodeState = NODE_STATE.FAILURE;
            return _nodeState;
        }

        public Selector(List<BehaviorTreeNode> children) : base(children) { }
    }
}
