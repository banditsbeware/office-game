using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using static System.Math;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class syncManagement : MonoBehaviour
{
    //configuration
    public Vector2 targetXBounds;
    public Vector2 targetYBounds;
    public Vector2 wordSpawnYBounds;
    //error time
    public float nice = .10f;
    public float good = .050f;
    public float great = .0250f;
    public float perfect = .01f;
    public float delayCorrection = .01f;

    //player input time
    public float attempt = 0f;

    //feedback
    public TMP_Text feedback;

    //prefabs
    public TMP_Text bossWord;
    public TMP_Text target;

    //holds multiple beats
    public Queue<Cue> cues = new Queue<Cue>();

    //player animation states
    public AK.Wwise.State deanSpeakingT;
    public AK.Wwise.State deanSpeakingF;
    public AK.Wwise.State marisolSpeakingT;
    public AK.Wwise.State marisolSpeakingF;
    public AK.Wwise.State marcSpeakingT;
    public AK.Wwise.State marcSpeakingF;

    //animation controllers
    public Animator dean_anim;
    public Animator marisol_anim;
    public Animator marc_anim;


    void Update()
    {
        if (cues.Count != 0)
        {
            if (Time.time - cues.Peek().cueTime > nice) //if word passes left bound, miss
            {
                Destroy(cues.Peek().target.gameObject);
                cues.Dequeue().text.color = new Color32(0, 0, 0, 225);
                feedback.text = "miss";
                feedback.color = new Color32(0, 0, 0, 255);

            }
        }

        UpdateAnimatorsFromState(dean_anim, deanSpeakingT, deanSpeakingF);
        UpdateAnimatorsFromState(marisol_anim, marisolSpeakingT, marisolSpeakingF);
        UpdateAnimatorsFromState(marc_anim, marcSpeakingT, marcSpeakingF);
    }

    //called when player clicks
    public void playerCue()
    {

        AkSoundEngine.PostEvent("Play_Player_60bmp_16ths", gameObject);
        if(cues.Count == 0)
        {
            feedback.text = "imagine clicking when there's nothing to click";
            feedback.color = new Color32(180, 0, 0, 255);
            feedback.fontSize = 32;
            return;
            
        }
        
        attempt = Time.time - delayCorrection;
        float nextBeat = cues.Peek().cueTime;
        Debug.Log(string.Format("Attempt Time: {0}, Beat Time: {1}", attempt, nextBeat));

        //calculate the scale of the margin of error
        if (Abs(attempt - nextBeat) < nice)
        {
            feedback.fontSize = 96;
            if (Abs(attempt - nextBeat) < perfect)
            {
                feedback.text = "perfect!";
                feedback.color = new Color32(220, 0, 255, 255);
            }
            else if (Abs(attempt - nextBeat) < great)
            {
                feedback.text = "great!";
                feedback.color = new Color32(0, 200, 0, 255);
            }
            else if (Abs(attempt - nextBeat) < good)
            {
                feedback.text = "good";
                feedback.color = new Color32(255, 255, 0, 255);
            }
            else
            {
                feedback.text = "okay";
                feedback.color = new Color32(200, 124, 0, 255);
            }
            Destroy(cues.Peek().target.gameObject);
            cues.Dequeue().text.color = new Color32(0, 0, 0, 225);
        }
        else
        {
            feedback.text = "boooo, you suck";
            feedback.color = new Color32(180, 0, 0, 255);
        }


    }

    //info about cue from Wwise
    public class Cue 
    {
        //cue is always 1 bar before actual input
        public float cueTime = 0f;
        public float barTime = 0f;
        public string word = "";
        public TMP_Text text;
        public TMP_Text target;

        public Cue(float cue, float bar, string text, syncManagement sync)
        {
            cueTime = cue;
            barTime = bar;
            word = text;

            //immediately create word on top side of screen, add a velocity downward so it hits center the moment you're supposed to click
            this.text = Instantiate(sync.bossWord, sync.transform.parent);
            this.target = Instantiate(sync.target, sync.transform.parent);
            this.text.text = word;
            this.target.text = word;

            this.text.transform.localPosition = ResituateWordPosition(sync);

            Vector2 targetPosition = new Vector2(this.text.transform.localPosition.x, Random.Range(sync.targetYBounds.x, sync.targetYBounds.y));
            this.target.transform.localPosition = targetPosition;

            this.text.GetComponent<wordMovement>().vel = new Vector2((targetPosition.x - this.text.transform.localPosition.x) / (barTime * 50), (targetPosition.y - this.text.transform.localPosition.y) / (barTime * 50));
        }

        private Vector2 ResituateWordPosition(syncManagement sync)
        {
            //pulls random Vector 2 within the camera viewport and randomized y position
            float y = Random.Range(sync.wordSpawnYBounds.x, sync.wordSpawnYBounds.y);
            return new Vector2(Random.Range(sync.targetXBounds.x, sync.targetXBounds.y), y);
            /* if (Random.Range(0, 1) > .5f)
            {
                return new Vector2 (1200, y);
            }

            return new Vector2 (-1200, y); */
        }
    }

    private void UpdateAnimatorsFromState(Animator animator, AK.Wwise.State AKTrueState, AK.Wwise.State AKFalseState)
    {
        uint currentStateID = 10;
        AkSoundEngine.GetState(AKTrueState.GroupId, out currentStateID);
        if (currentStateID == AKTrueState.Id)
        {
            animator.SetBool("isSpeaking", true);
        }
        else if (currentStateID == AKFalseState.Id)
        {
            animator.SetBool("isSpeaking", false);
        }
    }
}

