using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHim : MonoBehaviour {

    public float speed = 5f;
    public Rigidbody2D rb;
    private Vector2 v;
    [SerializeField] private UIManager UIMan;

    void Update()
    {
      v.x = Input.GetAxisRaw("Horizontal");   
      v.y = Input.GetAxisRaw("Vertical");   
    }

    void FixedUpdate()
    {
      if ((v.magnitude != 0) && (UIMan.gameState == "play")) 
      {
        rb.MovePosition(rb.position + v/v.magnitude * speed * Time.fixedDeltaTime); 
      }
    }

    // void OnTriggerEnter2D(Collider2D c) {
    //   c.GetComponent<TriggerScript>().changeColor();
    // }
}
