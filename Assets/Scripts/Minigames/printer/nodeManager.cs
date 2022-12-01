using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodeManager : MonoBehaviour
{

    private static LineRenderer line;
    public static GameObject activeNode;
    private node[] nodes;
    [SerializeField] private GameObject compLine;  //prefab of completed wire
    public List<string[]> completedWires;  //list of (name, name) completed wires
    public static Transform endpoint;  //gameObject to handle mouse tracking and node collision
    public static bool mouseOnNode;

    void OnEnable()
    {
        endpoint = transform.Find("endpoint");
        nodes = GetComponentsInChildren<node>();

        //when no node is selected, activeNode is the parent gameObject
        activeNode = gameObject;
        line = transform.GetComponentInChildren<LineRenderer>();
        completedWires = new List<string[]>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (mouseOnNode)
            {
                if (activeNode == gameObject) //if no node selected
                {
                    //create line by adding 2 points to LineRenderer
                    line.positionCount = 2;
                    activeNode = checkNodes();
                    line.SetPosition(0, activeNode.transform.localPosition);
                }
                else //one node selected, holding wire
                {
                    string clicked = checkNodes().name;

                    //duplicate prefab for completed wire
                    GameObject setWire = Instantiate(compLine, transform);
                    setWire.GetComponent<LineRenderer>().SetPositions(new Vector3[2]{activeNode.transform.localPosition, checkNodes().transform.localPosition});

                    //check if wire already exists, otherwise add both variations
                    if(!completedWires.Contains(new string[2] {activeNode.name, clicked})) 
                    {
                        completedWires.Add(new string[2] {activeNode.name, clicked});
                        completedWires.Add(new string[2] {clicked, activeNode.name});
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

        endpoint.position = mouseLocation();

        // update held wire
        if (activeNode != gameObject)
        {
            line.SetPosition(1, endpoint.localPosition);
        }
    }

    
    Vector3 mouseLocation()
    {
        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = 10; //distance of the plane from the camera
        return Camera.main.ScreenToWorldPoint(screenPoint); 
    }

    void releaseWire()
    {
        line.positionCount = 0;
        activeNode = gameObject;
    }

    //return gameObject that mouse is currently above
    GameObject checkNodes()
    {
        foreach (node n in nodes)
        {
            if (n.active)
            {
                return n.gameObject;
            }
        }
        
        return null;
    }

    node checkNodes(string returnNode)
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
