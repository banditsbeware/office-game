using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mirrorRealm_metalCollide : MonoBehaviour
{
    public Collider playerCollider;
    public AK.Wwise.Event MetalPing;
    public AK.Wwise.RTPC walkSpeed;
    
    public void OnCollisionEnter(Collision other) {
        if (other.collider == playerCollider)
        {
            walkSpeed.SetValue(gameObject, other.relativeVelocity.magnitude);
            MetalPing.Post(gameObject);
        }
    }
}
