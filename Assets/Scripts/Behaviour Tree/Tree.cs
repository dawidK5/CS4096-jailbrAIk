using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviourTree
{


    public abstract class Tree : MonoBehaviour
    {

        private Node root = null;
        // Start is called before the first frame update
        void Start()
        {
            root = SetupTree();
        }

        // Update is called once per frame
        void Update()
        {
            if (root != null)
            {
                root.Evaluate();
            }
        }

        protected abstract Node SetupTree();
    }

}