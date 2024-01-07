using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sheepCollision : MonoBehaviour
{
    private bool flag = false;

    // Start is called before the first frame update
    private void OnCollisionEnter(Collision other)
    {
        if(other.impulse.x < 0 && !flag)
        {
            flag = true;
            other.collider.GetComponent<sheep>().hitFence();
        }
    }
}
