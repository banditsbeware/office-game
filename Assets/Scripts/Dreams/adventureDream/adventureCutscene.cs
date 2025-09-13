using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.U2D;

//adventure cutscene needs the player obejct inside of the cart

public class adventureCutscene : cutscene
{
    [SerializeField] private float scrollV;
    [SerializeField] private Rigidbody2D bg;
    [SerializeField] private Rigidbody2D bg2;
    [SerializeField] private SpriteRenderer fg1SpriteRend;
    [SerializeField] private SpriteRenderer fg2SpriteRend;
    [SerializeField] private int xCrossoverValue;
    [SerializeField] private int xBGSpriteSize;
    [SerializeField] private Sprite bgTrans;
    [SerializeField] private Sprite fgTrans;
    [SerializeField] private GameObject fence;
    [SerializeField] private Sprite closedFence;
    
    
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

            // //sets the bg sprite that connects to the adventure map to the transitional sprite
            // if (bg.position.x < bg2.position.x)
            // {
            //     bg2.GetComponent<SpriteRenderer>().sprite = bgTrans;
            //     bg2.GetComponentInChildren<SpriteRenderer>().sprite = fgTrans;
            // }
            // else
            // {
            //     bg.GetComponent<SpriteRenderer>().sprite = bgTrans;
            //     bg.GetComponentInChildren<SpriteRenderer>().sprite = fgTrans;
            // }
            
        }

        if (bg.position.x > xCrossoverValue)
        {
            repo(bg);
            if (endFlag)
            {
                bg.GetComponent<SpriteRenderer>().sprite = bgTrans;
                fg1SpriteRend.sprite = fgTrans;
            }
        }
        if (bg2.position.x > xCrossoverValue)
        {
            repo(bg2);
            if (endFlag)
            {
                bg2.GetComponent<SpriteRenderer>().sprite = bgTrans;
                fg2SpriteRend.sprite = fgTrans;
            }
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

        fence.GetComponent<SpriteRenderer>().sprite = closedFence;
        fence.GetComponent<Rigidbody2D>().simulated = true;

    }

    //reposition backgrounds
    void repo(Rigidbody2D obj)
    {
        Vector2 offset = new Vector2 (2 * xBGSpriteSize, 0);
        obj.position -= offset;
    }
}