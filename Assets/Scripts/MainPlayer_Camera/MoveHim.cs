using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHim : MonoBehaviour {

    [SerializeField] private float speed = 1000f;
    
    //player's rigidbody
    private Rigidbody2D rb;

    //velocity
    private Vector2 v;
    public static Vector3 spawnLocation;

    void Awake()
    {
      rb = gameObject.GetComponent<Rigidbody2D>();
      transform.position = spawnLocation;
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
