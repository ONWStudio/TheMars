using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Onw.BehaviorTree
{
    public abstract class BehaviorTreeNode
    {
        public enum NODE_STATE
        {
            RUNNING,
            SUCCESS,
            FAILURE
        }

        public NODE_STATE NodeState => _nodeState;
        protected NODE_STATE _nodeState;

        public abstract NODE_STATE Evaluate();
    }
}
