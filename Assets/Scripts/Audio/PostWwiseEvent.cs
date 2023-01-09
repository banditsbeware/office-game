using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEvent : MonoBehaviour
{
    public void PostEvent(string eventName)
    {
        AkSoundEngine.PostEvent(eventName, gameObject);
    }
}
