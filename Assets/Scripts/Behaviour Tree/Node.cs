using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class Node
    {
        protected NodeState state;

        public Node parent;
        protected List<Node> children = new List<Node>();

        private Dictionary<string, object> dataContext = new Dictionary<string, object>();

        public Node() 
        {
            parent = null;

        }
        public Node(List<Node> children) 
        {
            foreach (Node child in children)
            {
                attach(child);
            }
        }

        private void attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;

        public void setData(string key, object value)
        {
            dataContext[key] = value;
        }

        public object getData(string key)
        {
            object value = null;
            if (dataContext.TryGetValue(key, out value))
                return value;

            Node node = parent;
            while (node != null)
            {
                value = node.getData(key);
                if (value != null)
                    return value;
                node = node.parent;
            }
            return null;
        }

        public bool clearData(string key)
        {
            object value = null;
            if (dataContext.ContainsKey(key))
            {
                dataContext.Remove(key);
                return true;
            }

            Node node = parent;
            while (node != null)
            {
                bool cleared = node.clearData(key);
                if (cleared)
                    return true;
                node = node.parent;
            }
            return false;
        }

    }

}
