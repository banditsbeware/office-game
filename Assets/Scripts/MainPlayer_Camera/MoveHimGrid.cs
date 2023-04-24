using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHimGrid : MonoBehaviour {
    
    //velocity
    public float speed = 1000f;
    private Vector3 velocity;

    //spawning
    public static string doorToExit = null;
    private door door;
    private Transform doorTransform;

    [SerializeField] private Animator animator;

    //used for position animation through code (mainly during "cutscenes")
    public bool isAnimating = false; 
    public Vector3 destination;

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
    }

    void FixedUpdate()
    {
      //check to see if player is on grid line
      bool x_check = Math.Abs(transform.position.x - (float)Math.Truncate(transform.position.x))  <= .01f || Math.Abs(transform.position.x - (float)Math.Truncate(transform.position.x)) >= .99f;
      bool y_check = Math.Abs(transform.position.y - (float)Math.Truncate(transform.position.y)) <= .01f || Math.Abs(transform.position.y - (float)Math.Truncate(transform.position.y)) >= .99f;

      Debug.Log(transform.position.x - (float)Math.Truncate(transform.position.x));

      //determine movement at grid line intersection
      if (x_check && y_check)
      {
        velocity = Vector2.zero;

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
          velocity.x = Input.GetAxisRaw("Horizontal") / 10f;
          animator.SetBool("moving", true);
        }
        else if (Input.GetAxisRaw("Vertical") != 0)
        {
          velocity.y = Input.GetAxisRaw("Vertical") / 10f;
          animator.SetBool("moving", true);
        }
        else
        {
          animator.SetBool("moving", false);
        }
      }

      //move
      transform.position += velocity;



        // if (isAnimating)
        // {
        //   rb.AddForce(velocity/velocity.magnitude * speed);
        // }
      

    }

    public Vector3 AnimatedVelocity(Vector3 destination)
    {
      Vector3 difference = destination - transform.position;

      Debug.Log(difference);

      if (difference.x < .05 && difference.y < .05)
      {
        isAnimating = false;
        difference = new Vector3(0, 0);
      }

      return Vector3.Normalize(difference);
    }

    public void AnimateMovement(Vector3 dest, float spd)
    {
      isAnimating = true;
      destination = dest;
      speed = spd;
    }

    public void Sleep()
    {
      animator.SetTrigger("sleep");
    }
    
}
