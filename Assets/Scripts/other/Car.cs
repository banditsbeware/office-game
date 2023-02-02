using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private float velocity = .1f;
    private Vector3 rightSpawnLocation = new Vector3(35, -.5f, 8);
    private Vector3 leftSpawnLocation = new Vector3(-5, -.5f, 7.5f);
    private void FixedUpdate() 
    {
        transform.position += new Vector3(velocity, 0, 0);
        if (transform.position.x < -5 || transform.position.x > 35)
        {
            Destroy(gameObject);
        }
    }

    private void Start() 
    {
        float rand = Random.Range(0f, 1f);
        if (rand <= .5)
        {
            rand += .5f;
            transform.position = rightSpawnLocation;
            velocity = -velocity * rand;
        }
        else
        {
            transform.position = leftSpawnLocation;
            velocity = velocity * rand;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }
}
