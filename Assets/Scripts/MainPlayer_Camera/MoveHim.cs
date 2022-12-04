using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHim : MonoBehaviour {

    [SerializeField] private float speed = 1000f;
    
    //player's rigidbody
    private Rigidbody2D rb;

    //velocity
    private Vector2 v;

    //spawning
    public static string doorToExit;
    private Transform door;
    private Vector3 spawnLocation;

    void Awake()
    {
      if(doorToExit != null) 
      {
        door = GameObject.Find(doorToExit).transform;
        transform.position = spawnLocation;
        if(doorToExit == "door_Street")
        {
          spawnLocation += new Vector3(0, 0, 1);
        }
      }


      rb = gameObject.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
      //get keyboard/controller input, put into Vector2 v
      v.x = Input.GetAxisRaw("Horizontal");   
      v.y = Input.GetAxisRaw("Vertical");   
    }

    void FixedUpdate()
    {
      //if playing, change the position of the regidbody
      if ((v.magnitude != 0) && (UIManager.gameState == "play")) 
      {
        rb.AddForce(v/v.magnitude * speed); 
      }
    }
}
