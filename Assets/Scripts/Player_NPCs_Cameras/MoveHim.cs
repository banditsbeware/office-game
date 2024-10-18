using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHim : MonoBehaviour {
    
    //player's rigidbody
    private Rigidbody2D rb;

    //velocity
    public float speed = 1000f;
    private Vector2 velocity;

    //spawning
    public static string doorToExit = null;
    private door door;
    private Transform doorTransform;

    [SerializeField] private Animator toupee;

    //used for position animation through code (mainly during "cutscenes")
    public bool isAnimating = false; 
    public Vector2 destination;


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
      velocity.x = Input.GetAxisRaw("Horizontal");   
      velocity.y = Input.GetAxisRaw("Vertical");

      if (isAnimating)
      {
        velocity = AnimatedVelocity(destination);

      }
    }

    void FixedUpdate()
    {
      //animations
      if (rb.velocity.magnitude < .5f)
      {
        toupee.SetBool("moving", false);
      }
      //if playing, change the position of the regidbody
      if ((velocity.magnitude != 0) && (UIManager.gameState == "play")) 
      {
        rb.AddForce(velocity/velocity.magnitude * speed);
        toupee.SetBool("moving", true);
      }
      if (isAnimating)
      {
        rb.AddForce(velocity/velocity.magnitude * speed);
      }

    }

    public Vector2 AnimatedVelocity(Vector2 destination)
    {
      Vector2 difference = destination - new Vector2(transform.position.x, transform.position.y);

      if (difference.x < .05 && difference.y < .05)
      {
        isAnimating = false;
        difference = new Vector2(0, 0);
      }
      return difference.normalized;
    }

    public void AnimateMovement(Vector2 dest, float spd)
    {
      isAnimating = true;
      destination = dest;
      speed = spd;
    }

    public void Sleep()
    {
      toupee.SetTrigger("sleep");
    }
    
}
