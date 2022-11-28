using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wiringNode : MonoBehaviour
{
    public static bool holding;
    private bool occupied;
    private GameObject activeNode = null;
    [SerializeField] private GameObject compLine;
    public LineRenderer line;
    public Transform endpoint;
    public void clickWire()
    {
        if (holding)
        {
            if (!occupied)
            {
                Instantiate(compLine, transform.parent).GetComponent<LineRenderer>().SetPositions(new Vector3[2]{activeNode.transform.position, gameObject.transform.position});
            }
        }
        else
        {
            line.positionCount = 2;
            line.SetPosition(0, transform.localPosition);
            activeNode = gameObject;
            holding = true;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && holding)
       {
            holding = false;
            line.positionCount = 0;
       }
        if (holding && activeNode == gameObject)
        {
            endpoint.position = moveWire();
            line.SetPosition(1, endpoint.localPosition);
        }
    }

    Vector3 moveWire()
    {
        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = 10; //distance of the plane from the camera
        return Camera.main.ScreenToWorldPoint(screenPoint); 
    }

}
