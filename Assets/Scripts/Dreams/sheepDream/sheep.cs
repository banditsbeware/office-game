using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;

public class sheep : MonoBehaviour
{
    public Vector3 sheepVelocity;
    private Rigidbody selfRigidbody;
    public Rigidbody fenceRigidbody;

    private Vector3 ejectVelMin = new Vector3 (-3, 2, -2);
    private Vector3 ejectVelMax = new Vector3 (3, 20, 2);
    private Vector3 ejectRotation = new Vector3 (2, 10, 1);



    void Start() //sets forward momentum of sheep when spawned
    {
        selfRigidbody = gameObject.GetComponent<Rigidbody>();
        selfRigidbody.velocity = sheepVelocity;
    }

    private void Update() {
        if (transform.position.x >= 25f)
        {
            Destroy(gameObject);
        }
    }

    public void hitFence() //called by fence trigger when hit, shoots sheep in random velocity+rotation
    {
        selfRigidbody.velocity = new Vector3(Random.Range(ejectVelMin.x, ejectVelMax.x), Random.Range(ejectVelMin.y, ejectVelMax.y), Random.Range(ejectVelMin.z, ejectVelMax.z));
        selfRigidbody.angularVelocity = new Vector3(Random.Range(-ejectRotation.x, ejectRotation.x), Random.Range(-ejectRotation.y, ejectRotation.y), Random.Range(-ejectRotation.z, ejectRotation.z));
    }

    public void jump()
    {
        Debug.Log("eek");
        selfRigidbody.velocity = new Vector3(selfRigidbody.velocity.x, 15, selfRigidbody.velocity.z);
    }
}
