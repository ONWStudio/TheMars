using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.BehaviorTree
{
    public class Sequence : CompositeNode
    {
        public override NODE_STATE Evaluate()
        {
            bool anyChildRunning = false;

            foreach (BehaviorTreeNode node in _children)
            {
                switch (node.Evaluate())
                {
                    case NODE_STATE.FAILURE:
                        _nodeState = NODE_STATE.FAILURE;
                        return _nodeState;
                    case NODE_STATE.SUCCESS:
                        continue;
                    case NODE_STATE.RUNNING:
                        anyChildRunning = true;
                        continue;
                    default:
                        _nodeState = NODE_STATE.SUCCESS;
                        return _nodeState;
                }
            }

            _nodeState = anyChildRunning ? NODE_STATE.RUNNING : NODE_STATE.SUCCESS;
            return _nodeState;
        }

        public Sequence(List<BehaviorTreeNode> children) : base(children) { }
    }
}
