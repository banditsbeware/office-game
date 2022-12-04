using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodeManager : MonoBehaviour
{
    private Printer printer;

    //node organization
    private node[] nodes;
    public static node activeNode;
    public List<NodeTuple> completedNodePairs;
    public List<GameObject> completedWires;
    public static bool mouseOnNode;

    //LineRendering
    private static LineRenderer line;
    [SerializeField] private GameObject compLine;  //prefab of completed wire
    public static Transform endpoint;  //gameObject to handle mouse tracking and node collision

    void OnEnable()
    {
        printer = transform.parent.GetComponent<Printer>();

        nodes = GetComponentsInChildren<node>();
        activeNode = null;
        completedNodePairs = new List<NodeTuple>();
        completedWires = new List<GameObject>();

        line = transform.GetComponentInChildren<LineRenderer>();
        endpoint = transform.Find("endpoint");
    }

    void OnDisable()
    {
        foreach (GameObject wire in completedWires)
        {
            Destroy(wire);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (mouseOnNode)
            {
                if (activeNode == null) //if no node selected
                {
                    //create line by adding 2 points to LineRenderer
                    line.positionCount = 2;
                    activeNode = clickedNode();
                    line.SetPosition(0, activeNode.transform.localPosition);
                }
                else //one node selected, holding wire
                {
                    //check if wire already exists
                    if(!completedNodePairs.Contains(new NodeTuple(activeNode, clickedNode())) && !completedNodePairs.Contains(new NodeTuple(clickedNode(), activeNode)))
                    {
                        completedNodePairs.Add(new NodeTuple(activeNode, clickedNode()));
                         
                        //duplicate prefab for completed wire
                        GameObject setWire = Instantiate(compLine, transform);
                        setWire.GetComponent<LineRenderer>().SetPositions(new Vector3[2]{activeNode.transform.localPosition, clickedNode().transform.localPosition});
                        completedWires.Add(setWire);

                        printer.currentError.wiresCompleted(activeNode.name, clickedNode().name);

                    }
                    releaseWire();
                }
            }
            else //mouse is not on a node
            {
                if (activeNode != gameObject) //you're holding a wire
                {
                    releaseWire();
                }
            }
        }

        endpoint.position = UIManager.mouseLocation();

        // update held wire
        if (activeNode != null)
        {
            line.SetPosition(1, endpoint.localPosition);
        }
    }

    void releaseWire()
    {
        line.positionCount = 0;
        activeNode = null;
    }

    //return gameObject that mouse is currently above
    node clickedNode()
    {
        foreach (node n in nodes)
        {
            if (n.active)
            {
                return n;
            }
        }
        return null;
    }
}
