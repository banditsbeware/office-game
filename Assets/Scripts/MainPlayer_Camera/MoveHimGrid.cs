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

    private int movemInt;

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
      //while moving
      if (movemInt != 0)
      {
        movemInt -= 1;

        transform.position += velocity;

        return;
      }

      velocity = Vector2.zero;
      animator.SetBool("moving", false);

      if (Input.GetAxisRaw("Horizontal") != 0)
      {
        velocity.x = Math.Sign(Input.GetAxisRaw("Horizontal")) * .1f;
        animator.SetBool("moving", true);
        movemInt = 9;
      }
      else if (Input.GetAxisRaw("Vertical") != 0)
      {
        velocity.y = Math.Sign(Input.GetAxisRaw("Vertical")) * .1f;
        animator.SetBool("moving", true);
        movemInt = 9;
      }

      transform.position += velocity;
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
