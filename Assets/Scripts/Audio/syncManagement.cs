using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using static System.Math;
using UnityEngine;
using TMPro;

public class syncManagement : MonoBehaviour
{
    //error time
    public float nice = .10f;
    public float good = .050f;
    public float great = .0250f;
    public float perfect = .01f;
    public float delayCorrection = .01f;
    private string debugText;

    //time management
    
    public float attempt = 0f;

    //feedback
    public TMP_Text feedback;
    public TMP_Text bossWord;
    public Queue<Cue> cues = new Queue<Cue>();

    void Update()
    {
        if (cues.Count != 0)
        {
            if (Time.time - cues.Peek().cueTime > nice)
            {
                cues.Dequeue().text.color = new Color32(0, 0, 0, 225);
                feedback.text = "miss";
                feedback.color = new Color32(0, 0, 0, 255);
            }
        }
    }

    public void playerCue()
    {
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
            cues.Dequeue().text.color = new Color32(0, 0, 0, 225);
        }
        else
        {
            feedback.text = "boooo, you suck";
            feedback.color = new Color32(180, 0, 0, 255);
        }


    }

    public class Cue 
    {
        public float cueTime = 0f;
        public float barTime = 0f;
        public string word = "";
        private syncManagement sync;
        public TMP_Text text;

        public Cue(float cue, float bar, string text, syncManagement sync)
        {
            cueTime = cue;
            barTime = bar;
            word = text;
            this.sync = sync;

            this.text = Instantiate(sync.bossWord, sync.transform.parent);
            this.text.text = word;
            this.text.GetComponent<wordMovement>().vel = -1200 / (barTime * 50);
        }
    }

    public void StartWord(float time, string word)
    {
        
    }
    
}

