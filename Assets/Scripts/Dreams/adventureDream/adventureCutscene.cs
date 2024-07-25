using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class adventureCutscene : cutscene
{
    [SerializeField] private float scrollV;
    [SerializeField] private Rigidbody2D bg;
    [SerializeField] private Rigidbody2D bg2;
    [SerializeField] private int xCrossoverValue;
    [SerializeField] private int xBGSpriteSize;
    
    
    private List<Rigidbody2D> scrollers = new List<Rigidbody2D>();

    public override void Start()
    {
        base.Start();
        StartCutscene();
    }

    public override void StartCutscene()
    {
        base.StartCutscene();
        scrollers.Add(bg);
        scrollers.Add(bg2);

        foreach (Rigidbody2D body in scrollers)
        {
            body.velocity = new Vector2(scrollV, 0f);
        }
        playerObject.GetComponent<MoveHim8bit>().isAnimating = true;
    }

    void Update()
    {
        if (endFlag)
        {
            if (bg.position.x > xCrossoverValue || bg2.position.x > xCrossoverValue)
            {
                foreach (Rigidbody2D body in scrollers)
                {
                    body.velocity = new Vector2(0, 0f);
                    
                }
                cutsceneAnimation.SetTrigger("endCutscene");
            }
            
        }

        if (bg.position.x > xCrossoverValue)
        {
            repo(bg);
        }
        if (bg2.position.x > xCrossoverValue)
        {
            repo(bg2);
        }
    }

    public override void EndCutscene()
    {
        base.EndCutscene();
    }

    public override void CutsceneFinished()
    {
        base.CutsceneFinished();
        playerObject.transform.SetParent(null);
        mainCamera.transform.SetParent(null);
        mainCamera.transform.position = playerObject.transform.position;
        dialogueObject.transform.SetParent(null);
        playerObject.GetComponent<MoveHim8bit>().isAnimating = false;
        playerObject.GetComponent<MoveHim8bit>().UpdateIntPosition();
    }

    //reposition backgrounds
    void repo(Rigidbody2D obj)
    {
        Vector2 offset = new Vector2 (2 * xBGSpriteSize, 0);
        obj.position -= offset;
    }
}