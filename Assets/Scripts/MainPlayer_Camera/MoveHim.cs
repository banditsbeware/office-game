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
    public static string doorToExit = null;
    private Transform door;
    private Vector3 spawnLocation;

    [SerializeField] private Animator toupee;

    void Awake()
    {
      
      if(doorToExit != null) 
      {
        door = GameObject.Find(doorToExit).transform;
      }
      
      if(doorToExit == "door_Street")
      {
        spawnLocation = door.position + new Vector3(0, 1, 0);
      }
      else if (doorToExit == "door_MainMenu")
      {
        spawnLocation = new Vector3(door.position.x, door.position.y, 0);
      }
      else
      {
        spawnLocation = new Vector3(door.position.x, 0, 9);
      }

      transform.position = spawnLocation;
      Debug.Log(doorToExit);
      

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
      //animations
      if (rb.velocity.magnitude < .5f)
      {
        toupee.SetBool("moving", false);
      }
      //if playing, change the position of the regidbody
      if ((v.magnitude != 0) && (UIManager.gameState == "play")) 
      {
        rb.AddForce(v/v.magnitude * speed);
        toupee.SetBool("moving", true);
      }
    }
}
