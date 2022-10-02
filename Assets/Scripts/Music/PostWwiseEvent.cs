using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;

public class PostWwiseEvent : MonoBehaviour
{
    public AK.Wwise.Event theEvent;

    public void PostEvent()
    {
        theEvent.Post(gameObject);
    }
}
