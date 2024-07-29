using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.BehaviorTree
{
    public class ActionNode : BehaviorTreeNode
    {
        private readonly Action _action = null;

        public override NODE_STATE Evaluate()
        {
            _action.Invoke();
            _nodeState = NODE_STATE.SUCCESS;
            return _nodeState;
        }

        public ActionNode(Action action)
        {
            _action = action;
        }
    }
}