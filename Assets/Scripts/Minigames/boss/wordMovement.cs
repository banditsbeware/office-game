using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//boss minigame words flying across screen
public class wordMovement : MonoBehaviour
{
    public Vector2 vel = new Vector2(0, 0);
    
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localPosition = new Vector3(transform.localPosition.x + vel.x, transform.localPosition.y + vel.y, 0f);
    }
}
