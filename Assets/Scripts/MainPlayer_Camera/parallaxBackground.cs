using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallaxBackground : MonoBehaviour
{
    
    [SerializeField] private Transform bg;
    [SerializeField] private Transform bg2;
    [SerializeField] private Transform mainCamera;
    public Vector3 movementSpeed;
    private float bumpAmount = 250f; //amount to move when one sprite needs to be reset, double the lenght of a single sprite  
    private float gateAmount = -100f;  //value at which sprite will jump

    void FixedUpdate()
    {
        bg.localPosition += movementSpeed;
        bg2.localPosition += movementSpeed;
    }

    void Update()
    {
        if (bg.localPosition.x < gateAmount)
        {
            repo(bg);
        }
        if (bg2.localPosition.x < gateAmount)
        {
            repo(bg2);
        }

        transform.position = new Vector3(mainCamera.position.x, transform.position.y, transform.position.z);
    }

    void repo(Transform obj)
    {
        Vector3 offset = new Vector3 (bumpAmount, 0, 0);
        obj.localPosition += offset;
    } 
}


