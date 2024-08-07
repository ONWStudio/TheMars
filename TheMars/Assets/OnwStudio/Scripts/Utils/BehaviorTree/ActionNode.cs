using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.BehaviorTree
{
    public class ActionNode : BehaviorTreeNode
    {
        private readonly Func<NODE_STATE> _action = null;

        public override NODE_STATE Evaluate()
        {
            return _action?.Invoke() ?? NODE_STATE.FAILURE;
        }

        public ActionNode(Func<NODE_STATE> action)
        {
            _action = action;
        }
    }
}