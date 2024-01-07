using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalBubble : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        transform.parent.GetComponent<WanderingNPC>().BubbleEntered(other);
    }
}
