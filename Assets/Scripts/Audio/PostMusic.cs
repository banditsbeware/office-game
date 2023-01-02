using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostMusic : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event playBossMusicEvent;
    [SerializeField] private AK.Wwise.Event stopBossMusicEvent;
    public syncManagement sync;

    void OnEnable()
    {
        playBossMusicEvent.Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, CallCueSync);
    }

    void OnDisable()
    {
        stopBossMusicEvent.Post(gameObject);
    }

    void CallCueSync(object in_cookie, AkCallbackType in_type, object in_info)
    {
        AkMusicSyncCallbackInfo info = (AkMusicSyncCallbackInfo)in_info;
        sync.cues.Enqueue(new syncManagement.Cue(Time.time + info.segmentInfo_fBarDuration, info.segmentInfo_fBarDuration, info.userCueName, sync));
    }


}
