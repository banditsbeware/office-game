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
    private door door;
    private Transform doorTransform;

    [SerializeField] private Animator toupee;

    void Awake()
    {
      //always have a door to exit from
      if (doorToExit == "door_MainMenu")
      {
        doorToExit = null;
      }
      if (doorToExit == null) 
      {
        doorToExit = "door_origin";
      }
      //find the door you're exiting from
      door = GameObject.Find(doorToExit).GetComponent<door>();
      doorTransform = door.transform;

      
      transform.position = doorTransform.position + door.spawnPointOffset;

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
