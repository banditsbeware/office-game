using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostMusic : MonoBehaviour
{
    public AK.Wwise.Event MusicEvent;
    public syncManagement sync;

    void Start()
    {
        MusicEvent.Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, CallCueSync);
    }

    void CallCueSync(object in_cookie, AkCallbackType in_type, object in_info)
    {
        AkMusicSyncCallbackInfo info = (AkMusicSyncCallbackInfo)in_info;
        sync.cues.Enqueue(new syncManagement.Cue(Time.time + info.segmentInfo_fBarDuration, info.segmentInfo_fBarDuration, info.userCueName, sync));
    }


}
