using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHim8bit : MonoBehaviour {
    

    //velocity
    public Vector2Int pos = new Vector2Int(0, 0);
    public float speed = 1000f;
    public Vector2Int velocity;

    //spawning
    public static string doorToExit = null;
    private door door;
    private Transform doorTransform;

    [SerializeField] private Animator animator;

    //used for door passage
    public bool isNode;
    public bool skipNode;
    

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
      //while not in grid
      if (pos.x % 10 != 0 || pos.y % 10 != 0)
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
        animator.SetBool("moving", true);
        ExecuteMovementFrame();
      }
      else if (Input.GetAxisRaw("Vertical") != 0)
      {
        velocity.y = (int) Input.GetAxisRaw("Vertical");
        animator.SetBool("moving", true);
        ExecuteMovementFrame();
      }      
    }

    void ExecuteMovementFrame()
    {
      pos += velocity;
      Debug.Log("moved!");
      transform.position = new Vector3((float) pos.x / 10f, (float) pos.y / 10f, 0);

      return;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
      velocity = new Vector2Int(-velocity.x, -velocity.y);
    }
}
