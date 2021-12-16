using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToupeeScript : MonoBehaviour
{
    private Animator anim;

    void Start() {
      anim = gameObject.GetComponent<Animator>() as Animator;
      anim.speed = 0;
    }
}