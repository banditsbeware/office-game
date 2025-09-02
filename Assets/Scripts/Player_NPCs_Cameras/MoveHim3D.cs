using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHim3D : MonoBehaviour {

    [SerializeField] private float force = 1000f;
    
    //player's rigidbody
    private Rigidbody rb;
    //velocity
    private Vector3 forceDir;
    //public static string doorToExit = null;  this remains in MoveHim
    private door door;
    private Transform doorTransform;

    [SerializeField] private Animator toupee;

    void Awake()
    {
      //always have a door to exit from
      if (MoveHim.doorToExit == "door_MainMenu")
      {
        MoveHim.doorToExit = null;
      }
      if (MoveHim.doorToExit == null) 
      {
        MoveHim.doorToExit = "door_origin";
      }
      //find the door you're exiting from
      door = GameObject.Find(MoveHim.doorToExit).GetComponent<door>();
      doorTransform = door.transform;
      
      transform.position = doorTransform.position + door.spawnPointOffset;

      rb = gameObject.GetComponent<Rigidbody>();
    }
    void Update()
    {
      //get keyboard/controller input, put into Vector2 v
      forceDir.x = Input.GetAxisRaw("Horizontal");   
      forceDir.z = Input.GetAxisRaw("Vertical");   
    }

    void FixedUpdate()
    {
      //animations
      if (rb.velocity.magnitude < .5f)
      {
        toupee.SetBool("moving", false);
      }
      //if playing, change the position of the regidbody
      if ((forceDir.magnitude != 0) && (UIManager.gameState == "play")) 
      {
        rb.AddForce(forceDir/forceDir.magnitude * force);
        toupee.SetBool("moving", true);
      }
    }
}
