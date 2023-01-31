using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class literallyFlappyBird : MonoBehaviour
{
    [SerializeField] private float jumpValue;
    [SerializeField] private float scrollV;
    [SerializeField] private float pipeBounds;
    private bool jump;
    private bool readyTGo;
    private float numOfPipes = 5f;
    [System.NonSerialized] public bool inGame;
    [SerializeField] private Rigidbody2D bird;
    [SerializeField] private Rigidbody2D bg;
    [SerializeField] private Rigidbody2D bg2;
    [SerializeField] private GameObject pipe;
    private int score;
    [SerializeField] private TMP_Text scoreTxt;
    [SerializeField] private TMP_Text menu;
    [SerializeField] private GameObject referenceMini;
    [SerializeField] private fingerClicky clickMang;
    private List<Rigidbody2D> scrollers = new List<Rigidbody2D>();
    private Queue<GameObject> pipes = new Queue<GameObject>();
    //Wwise
    public AK.Wwise.Event flap;
    
    public void Start()
    {
        scrollers.Add(bg);
        scrollers.Add(bg2);

        inGame = false;
        readyTGo = true;

        bird.transform.localPosition = new Vector2(0f, 0f);
        bird.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        bird.simulated = false;

        menu.text = "bird game\n\n\nhigh score:\n" + meta.flappyHighScore;

        for (int i = 0; i < numOfPipes; i++)
        {
            GameObject pip = Instantiate(pipe, gameObject.transform);
            pip.transform.localPosition = new Vector3(40f + i * 20f, Random.Range(-pipeBounds, pipeBounds), 0f);
            pipes.Enqueue(pip);
            scrollers.Add(pip.GetComponent<Rigidbody2D>());
        }

        menu.gameObject.SetActive(true);
        scoreTxt.gameObject.SetActive(false);
    }

    void Update()
    {
        if(referenceMini.activeSelf) 
        {
            if (inGame)
            {
                bird.rotation = bird.velocity.y;
                if (isJumping())
                {
                    bird.velocity = new Vector2(0, jumpValue);
                    flap.Post(gameObject);
                }
            }
            else
            {
                if (isJumping() && readyTGo)
                {
                    Begin();
                    flap.Post(gameObject);
                }
            }
    
            if (bg.position.x < -35)
            {
                repo(bg);
            }
            if (bg2.position.x < -35)
            {
                repo(bg2);
            }
        }
    }

    //reposition backgrounds
    void repo(Rigidbody2D obj)
    {
        Vector2 offset = new Vector2 (80f, 0);
        obj.position += offset;
    }

    void Reset()
    {
        for (int i = 0; i < pipes.Count; i++)
        {
            GameObject pip = pipes.Dequeue();
            pip.transform.localPosition = new Vector3(40f + i * 20f, Random.Range(-pipeBounds, pipeBounds), 0f);
            pip.GetComponent<Rigidbody2D>().velocity += new Vector2(-2f, 0);
            pipes.Enqueue(pip);
        }

    }

    public void ExitBath()
    {
        for (int i = 0; i < pipes.Count; i++)
        {
            GameObject pip = pipes.Dequeue();
            pip.transform.localPosition = new Vector3(40f + i * 20f, Random.Range(-pipeBounds, pipeBounds), 0f);
            pipes.Enqueue(pip);
        }
        
        bird.transform.localPosition = new Vector2(0f, 0f);
        bird.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        bird.simulated = false;
        inGame = false;
    }

    //create scrolling objects, pipes, and set score, velocities, etc.
    void Begin()
    {
        foreach (Rigidbody2D body in scrollers)
        {
            body.velocity = new Vector2(scrollV, 0f);
        }

        Reset();
        
        bird.transform.localPosition = Vector2.zero;
        bird.velocity = new Vector2(0, jumpValue);
        bird.angularVelocity = 0;
        bird.simulated = true;

        inGame = true;
        score = 0;
        scoreTxt.text = "Score: " + score;
        menu.gameObject.SetActive(false);
        scoreTxt.gameObject.SetActive(true);
    }

    //moving pipes
    void FixedUpdate()
    {
        if (inGame && pipes.Count != 0)
        {
            if ((pipes.Peek().transform.position.x < -30))
            {
                float offset = numOfPipes * 20f;
                GameObject first = pipes.Dequeue();
                first.transform.localPosition = new Vector2(first.transform.position.x + offset, Random.Range(-pipeBounds, pipeBounds));
                pipes.Enqueue(first);
            }
        }
    }

    //buffer after game loss
    IEnumerator waitToPlay()
    {
        yield return new WaitForSeconds(.5f);
        readyTGo = true;
    }
    
    public void gameOver()
    {
        inGame = false;
        readyTGo = false;
        StartCoroutine(waitToPlay());

        bird.velocity = new Vector2(Random.Range(-10f, 10f), 30f);
        bird.angularVelocity = Random.Range(-180f, 180f);

        foreach (Rigidbody2D obj in scrollers)
        {
            obj.velocity = Vector2.zero;
        }

        if (score > meta.flappyHighScore)
        {
            meta.flappyHighScore = score;
            menu.text = "bird game\n\n\nhigh score:\n" + meta.flappyHighScore;
        }

        menu.gameObject.SetActive(true);
        scoreTxt.gameObject.SetActive(false);
    }

    public void Score()
    {
        score++;
        scoreTxt.text = "Score: " + score;
    }

    private bool isJumping()
    {
        if (clickMang.clicked || Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}