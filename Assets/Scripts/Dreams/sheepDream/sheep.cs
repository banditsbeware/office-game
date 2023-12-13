using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sheep : MonoBehaviour
{
    public Vector3 sheepVelocity;
    private Rigidbody selfRigidbody;
    public Rigidbody fenceRigidbody;


    void Start()
    {
        selfRigidbody = gameObject.GetComponent<Rigidbody>();
        selfRigidbody.velocity = sheepVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
