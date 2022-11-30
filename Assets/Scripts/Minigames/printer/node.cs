using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class node : MonoBehaviour
{
    [System.NonSerialized] public bool active;

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
