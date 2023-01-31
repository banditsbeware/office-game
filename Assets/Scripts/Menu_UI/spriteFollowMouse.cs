using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteFollowMouse : MonoBehaviour
{
    //for use with standin sprite to track mouse position
    void Update()
    {
        transform.position = UIManager.mouseLocation();
    }
}
