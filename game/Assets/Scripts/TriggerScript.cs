using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour {

  private BoxCollider2D bc;
  private SpriteRenderer sp;

  private GameObject[] toupeeHavers;

  public void changeColor() {
    sp.color = new Color(Random.value, Random.value, Random.value);
  }

  void Awake() {
    sp = gameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
    bc = gameObject.AddComponent<BoxCollider2D>() as BoxCollider2D;
    bc.size = new Vector2(1.0f, 1.0f);
    bc.isTrigger = true;
  }

  void OnTriggerEnter2D(Collider2D c) {

    GameObject tm = GameObject.Find("ToupeeMan");
    tm.GetComponent<Animator>().speed = 1;

    c.GetComponent<MoveHim>().stopAnimation();
  }

  void OnTriggerExit2D(Collider2D c) {

    GameObject tm = GameObject.Find("ToupeeMan");
    tm.GetComponent<Animator>().speed = 0;

    c.GetComponent<MoveHim>().startAnimation();
  }
}
