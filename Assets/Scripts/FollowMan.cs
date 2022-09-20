using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMan : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float smoothSpeed = .125f;
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 desiredPos = player.transform.position + offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
        transform.position = smoothedPos;
    }
}
