using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class literallyFlappyBird : MonoBehaviour
{
    public float jumpValue;
    public float scrollV;
    public float pipeBounds;
    private bool jump;
    private bool readyTGo;
    private float numOfPipes = 5f;
    [System.NonSerialized] public bool inGame;
    public Rigidbody2D bird;
    public Rigidbody2D bg;
    public Rigidbody2D bg2;
    public GameObject pipe;
    public int score;
    public TMP_Text scoreTxt;
    public TMP_Text menu;
    private List<Rigidbody2D> scrollers = new List<Rigidbody2D>();
    private Queue<GameObject> pipes = new Queue<GameObject>();
    
    //Wwise
    public AK.Wwise.Bank Bird;
    public AK.Wwise.Event flap;
    public AK.Wwise.Event chime;
    
    void OnEnable()
    {
        scrollers.Add(bg);
        scrollers.Add(bg2);

        inGame = false;
        readyTGo = true;

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
        if (inGame)
        {
            bird.rotation = bird.velocity.y;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                bird.velocity = new Vector2(0, jumpValue);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && readyTGo)
            {
                Begin();
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

    void Begin()
    {
        foreach (Rigidbody2D body in scrollers)
        {
            body.velocity = new Vector2(scrollV, 0f);
        }

        for (int i = 0; i < pipes.Count; i++)
        {
            GameObject pip = pipes.Dequeue();
            pip.transform.localPosition = new Vector3(40f + i * 20f, Random.Range(-pipeBounds, pipeBounds), 0f);
            pip.GetComponent<Rigidbody2D>().velocity += new Vector2(-2f, 0);
            pipes.Enqueue(pip);
        }

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

    void FixedUpdate()
    {
        if (inGame && pipes.Count != 0)
        {
            if ((pipes.Peek().transform.position.x < -30))
            {
                redoPipe();
            }
        }
    }

    void repo(Rigidbody2D obj)
    {
        Vector2 offset = new Vector2 (80f, 0);
        obj.position += offset;
    }

    IEnumerator waitToPlay()
    {
        yield return new WaitForSeconds(.5f);
        readyTGo = true;
    }
    void redoPipe()
    {
        float offset = numOfPipes * 20f;
        GameObject first = pipes.Dequeue();
        first.transform.localPosition = new Vector2(first.transform.position.x + offset, Random.Range(-pipeBounds, pipeBounds));
        pipes.Enqueue(first);
    }

    public void gameOver()
    {
        inGame = false;
        readyTGo = false;
        StartCoroutine(waitToPlay());

        foreach (Rigidbody2D obj in scrollers)
        {
            obj.velocity = Vector2.zero;
            bird.velocity = new Vector2(Random.Range(-10f, 10f), 30f);
            bird.angularVelocity = Random.Range(-180f, 180f);
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
}