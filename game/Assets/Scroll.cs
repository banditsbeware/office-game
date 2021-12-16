using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour {

    public float speed = 0.1f;

    void Update() {

        float offset = Time.time * speed;

        GetComponent<Renderer>().material.mainTextureOffset = new Vector2( offset, -offset );
        
    }
}
