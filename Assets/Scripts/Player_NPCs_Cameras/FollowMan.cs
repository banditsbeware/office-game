using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMan : MonoBehaviour
{
    [SerializeField] private bool smoothFollow;
    public bool inCutscene;
    [SerializeField] private Vector3 xyzMin = new Vector3(0, -5, -10);
    [SerializeField] private Vector3 xyzMax = new Vector3(9, 8, -10);
    public Vector3 cutscenePos = new Vector3(0, 0, 0);
    private Vector3 desiredPos = new Vector3(0, 0, 0);


    public Transform player;
    public Vector3 offset;
    public float smoothSpeed = .125f;

    void Start()
    {
        if (inCutscene)
        {
            transform.position = cutscenePos;
            return;
        }

        transform.position = player.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        desiredPos = player.transform.position + offset;

        if (inCutscene)
        {
            transform.position = cutscenePos;
            return;
        }

        if (!smoothFollow)
        {
            transform.position = desiredPos;
            return;
        }

        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
        transform.position = Clamp(smoothedPos, xyzMin, xyzMax);
    }
        

    // public static int Clamp(int value, int min, int max)
    // {
    //     return (value < min) ? min : (value > max) ? max : value;
    // }

    //Keep an input Vector3 within a range
    public static Vector3 Clamp(Vector3 vector, Vector3 min, Vector3 max)
    {
        Vector3 clampd = new Vector3(0, 0, 0);
        
        for (int i = 0; i < 3; i++)
        {
            if (vector[i] <= min[i])
            {
                clampd[i] = min[i];
            } 
            else if (vector[i] >= max[i])
            {
                clampd[i] = max[i];
            } 
            else 
            {
                clampd[i] = vector[i];
            }
        }

        return  clampd;
    }
}

