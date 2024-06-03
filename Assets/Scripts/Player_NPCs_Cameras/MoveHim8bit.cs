using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHim8bit : MonoBehaviour {
    //velocity
    public Vector2Int intPosition = new Vector2Int(0, 0);
    public Vector2Int velocity;
    public int speed = 10; //also the number of fixedUpdates per grid point
    

    //spawning
    public static string doorToExit = null;
    private door door;
    private Transform doorTransform;

    [SerializeField] private Animator animator;

    //used for door passage
    public bool isNode;
    public bool skipNode;

    //animation & cutscenes
    public bool isAnimating = false; 
    

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

      intPosition = new Vector2Int((int) transform.position.x * speed, (int) transform.position.y * speed);
    }

    void FixedUpdate()
    {
      //while not in grid
      if (intPosition.x % speed != 0 || intPosition.y % speed != 0)
      {
        isNode = false;
        ExecuteMovementFrame();
        return;
      }
      if (skipNode)
      {
        ExecuteMovementFrame();
        skipNode = false;
        return;
      }

      velocity = Vector2Int.zero;
      animator.SetBool("moving", false);
      isNode = true;


      //executes if on a grid intersection
      if (Input.GetAxisRaw("Horizontal") != 0)
      {
        velocity.x = (int) Input.GetAxisRaw("Horizontal");
        Collider2D hitCollider = Physics2D.OverlapCircle(new Vector2(transform.position.x + velocity.x, transform.position.y), 0f);
        ExecuteMovementFrame();
        
        
      }
      else if (Input.GetAxisRaw("Vertical") != 0)
      {
        velocity.y = (int) Input.GetAxisRaw("Vertical");
        Collider2D hitCollider = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + velocity.y), 0f);
        ExecuteMovementFrame();
      }      
    }

    void ExecuteMovementFrame()
    {
      if (!isAnimating)
      {
        animator.SetBool("moving", true);
        
        intPosition += velocity;
        transform.position = new Vector3((float) intPosition.x / (float) speed, (float) intPosition.y / (float) speed, 0);
      }

      return;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
      velocity = new Vector2Int(-velocity.x, -velocity.y);
    }
}
