using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sheepCollision : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision other)
    {
        other.collider.GetComponent<sheep>().hitFence();
    }
}
