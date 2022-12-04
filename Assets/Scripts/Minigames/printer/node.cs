using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class node : MonoBehaviour
{
    [System.NonSerialized] public bool active; //mouse is above

    void OnEnable()
    {
        active = false;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == nodeManager.endpoint.GetComponent<Collider2D>())
        {
            nodeManager.mouseOnNode = true;
            active = true;
        }
        
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other == nodeManager.endpoint.GetComponent<Collider2D>())
        {
            nodeManager.mouseOnNode = false;
            active = false;
        }
    }
}

public class NodeTuple : System.IEquatable<NodeTuple>
{
    public (node, node) pair;

    public NodeTuple(node n1, node n2)
    {
        pair.Item1 = n1;
        pair.Item2 = n2;
    }

    public bool Equals(NodeTuple other)
    {
        if (other == null)
        {
            return false;
        }

        if ((this.pair.Item1 != other.pair.Item1) || (this.pair.Item2 != other.pair.Item2))
        {
            return false;
        }

        return true;
    }
}
