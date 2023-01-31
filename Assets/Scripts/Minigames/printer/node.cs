using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class node : MonoBehaviour
{
    [System.NonSerialized] public bool active; //true when mouse is above node

    void OnEnable()
    {
        active = false;
    }


    void OnTriggerEnter2D(Collider2D other) //endpoint is tracked with mouse, so when mouse collides with node
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
 
//for keeping a list of pairs in nodeManager
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
