using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHim : MonoBehaviour {

    public float speed = 5f;

    public Rigidbody2D rb;

    // private BoxCollider2D bc;
    // private Animator anim;

    Vector2 v;

  //  public void stopAnimation() { anim.speed = 0; }

  //  public void startAnimation() { anim.speed = 1; }

    void Awake() {
    //   anim = gameObject.GetComponent<Animator>() as Animator; 
    //   bc = gameObject.AddComponent<BoxCollider2D>() as BoxCollider2D;
    //   bc.isTrigger = true;
    }

    void Update() {
      v.x = Input.GetAxisRaw("Horizontal");   
      v.y = Input.GetAxisRaw("Vertical");   
    }

    void FixedUpdate() {
      if (v.magnitude != 0) 
        rb.MovePosition(rb.position + v/v.magnitude * speed * Time.fixedDeltaTime); 
    }

    // void OnTriggerEnter2D(Collider2D c) {
    //   c.GetComponent<TriggerScript>().changeColor();
    // }
}
