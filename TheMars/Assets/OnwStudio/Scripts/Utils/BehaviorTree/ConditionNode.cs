using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.BehaviorTree
{
    public class ConditionNode : BehaviorTreeNode
    {
        private readonly Func<bool> _condition = null;

        public override NODE_STATE Evaluate()
        {
            return _condition.Invoke() ? NODE_STATE.SUCCESS : NODE_STATE.FAILURE;
        }

        public ConditionNode(Func<bool> condition)
        {
            _condition = condition;
        }
    }
}