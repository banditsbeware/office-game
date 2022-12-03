using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteFollowMouse : MonoBehaviour
{
    void Update()
    {
        transform.position = UIManager.mouseLocation();
    }
}
