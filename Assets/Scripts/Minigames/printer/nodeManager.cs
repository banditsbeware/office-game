using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodeManager : MonoBehaviour
{
    private static LineRenderer line;
    public static GameObject activeNode;
    private node[] nodes;
    [SerializeField] private GameObject compLine;
    public List<string[]> completedWires;
    public static Transform endpoint;
    public static bool mouseOnNode;

    void OnEnable()
    {
        endpoint = transform.Find("endpoint");
        nodes = GetComponentsInChildren<node>();
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
                if (activeNode == gameObject)
                {
                    line.positionCount = 2;
                    activeNode = checkNodes();
                    line.SetPosition(0, activeNode.transform.localPosition);
                }
                else
                {
                    string clicked = checkNodes().name;

                    GameObject setWire = Instantiate(compLine, transform);
                    setWire.GetComponent<LineRenderer>().SetPositions(new Vector3[2]{activeNode.transform.localPosition, checkNodes().transform.localPosition});

                    if(!completedWires.Contains(new string[2] {activeNode.name, clicked})) 
                    {
                        completedWires.Add(new string[2] {activeNode.name, clicked});
                        completedWires.Add(new string[2] {clicked, activeNode.name});
                    }
                    releaseWire();
                }
            }
            else
            {
                if (activeNode != gameObject)
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
