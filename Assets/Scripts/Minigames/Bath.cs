using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bath : MonoBehaviour
{
    void OnEnable()
    {
        AkSoundEngine.PostEvent("Play_Bath_Amb", gameObject);
        AkSoundEngine.SetState("room", "bath");
    }
    
    void OnDisable()
    {
        AkSoundEngine.PostEvent("Stop_Bath_Amb", gameObject);
        AkSoundEngine.SetState("room", "office");
    }
}
