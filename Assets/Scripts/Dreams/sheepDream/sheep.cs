using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;

public class sheep : MonoBehaviour
{
    public Vector3 sheepVelocity;
    private Rigidbody selfRigidbody;
    private Collider boundCollider;

    private sheepController parentController;

    [SerializeField] private Sprite[] sheepSprites;

    private bool cleared = false;

    //values for velocities/rotation when hit fence
    private Vector3 ejectVelMin = new Vector3 (-3, 2, -2);
    private Vector3 ejectVelMax = new Vector3 (3, 20, 2);
    private Vector3 ejectRotation = new Vector3 (2, 10, 1);



    void Start() //sets forward momentum of sheep when spawned
    {
        selfRigidbody = gameObject.GetComponent<Rigidbody>();
        selfRigidbody.velocity = sheepVelocity;
        parentController = transform.parent.GetComponent<sheepController>();
        boundCollider = GameObject.Find("fencingBound").GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other) {
        if (!cleared && other == boundCollider)
        {
            cleared = true;
            parentController.sheepCleared();
            GetComponent<Renderer>().sortingOrder = 0;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other == boundCollider)
        {
            GetComponent<Renderer>().sortingOrder = 2;
        }
    }

    public void hitFence() //called by fence trigger when hit, shoots sheep in random velocity+rotation
    {
        selfRigidbody.velocity = new Vector3(Random.Range(ejectVelMin.x, ejectVelMax.x), Random.Range(ejectVelMin.y, ejectVelMax.y), Random.Range(ejectVelMin.z, ejectVelMax.z));
        selfRigidbody.angularVelocity = new Vector3(Random.Range(-ejectRotation.x, ejectRotation.x), Random.Range(-ejectRotation.y, ejectRotation.y), Random.Range(-ejectRotation.z, ejectRotation.z));
        Camera.main.GetComponent<FollowMan>().player = transform;
        Camera.main.GetComponent<FollowMan>().desiredSize = 10;
    }

    public void jump()
    {
        gameObject.GetComponent<Animator>().SetTrigger("jump");
        selfRigidbody.velocity = new Vector3(selfRigidbody.velocity.x, 15, selfRigidbody.velocity.z);
    }
}
