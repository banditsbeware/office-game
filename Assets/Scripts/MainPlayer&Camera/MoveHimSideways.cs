using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHimSideways : MonoBehaviour {

    public float speed = 5f;
    public static float location;
    private Rigidbody2D rb;

    private Vector2 v;

    void Awake()
    {
      rb = gameObject.GetComponent<Rigidbody2D>();
      transform.position = new Vector3(location, 0f, 0f);
    }
    void Update()
    {
      //get keyboard/controller input, put into Vector2 v
      v.x = Input.GetAxisRaw("Horizontal");   
    }

    void FixedUpdate()
    {
      //if playing, change the position of the regidbody
      if ((v.magnitude != 0) && (UIManager.gameState == "play")) 
      {
        rb.MovePosition(rb.position + v/v.magnitude * speed * Time.fixedDeltaTime); 
      }
    }
}