using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHim : MonoBehaviour {

    public float speed = 5f;
    
    //player's rigidbody
    private Rigidbody2D rb;

    //velocity
    private Vector2 v;
    [SerializeField] private UIManager UIMan;
    void Awake()
    {
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

    // void OnTriggerEnter2D(Collider2D c) {
    //   c.GetComponent<TriggerScript>().changeColor();
    // }
}
