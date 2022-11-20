using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wordMovement : MonoBehaviour
{
    public float vel = 0f;
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localPosition = new Vector3(transform.localPosition.x + vel, transform.localPosition.y, 0f);
    }
}
