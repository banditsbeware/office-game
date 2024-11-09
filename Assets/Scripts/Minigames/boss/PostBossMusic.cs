using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostBossMusic : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event playBossMusicEvent;
    [SerializeField] private AK.Wwise.Event stopBossMusicEvent;
    public syncManagement sync;

    //posts music event and sets up callback
    void OnEnable()
    {
        if(transform.parent.GetComponentInParent<interact>().isInteractable) 
        {
            playBossMusicEvent.Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, CallCueSync);
        }
    }

    void OnDisable()
    {
        if(transform.parent.GetComponentInParent<interact>().isInteractable) 
        {
            stopBossMusicEvent.Post(gameObject);
        }
    }

    //called every CueSync in Wwise
    void CallCueSync(object in_cookie, AkCallbackType in_type, object in_info)
    {
        Debug.Log(in_type);
        AkMusicSyncCallbackInfo info = (AkMusicSyncCallbackInfo)in_info;
        sync.cues.Enqueue(new syncManagement.Cue(Time.time + info.segmentInfo_fBarDuration, info.segmentInfo_fBarDuration, info.userCueName, sync));
    }


}
