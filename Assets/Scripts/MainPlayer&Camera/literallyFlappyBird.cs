using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class literallyFlappyBird : MonoBehaviour
{
    public float jumpValue;
    public float scrollV;
    public float pipeBounds;
    private bool jump;
    public bool inGame;
    public Rigidbody2D bird;
    public Rigidbody2D bg;
    public Rigidbody2D bg2;
    public GameObject pipe;
    private List<Rigidbody2D> scrollers = new List<Rigidbody2D>();
    private Queue<GameObject> pipes = new Queue<GameObject>();

    void OnEnable()
    {
        scrollers.Add(bg);
        scrollers.Add(bg2);

        inGame = false;
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("began");
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
        if (pipes.Count != 0)
        {
            for (int i = 0; i < pipes.Count; i++)
            {
                GameObject pip = pipes.Dequeue();
                scrollers.Remove(pip.GetComponent<Rigidbody2D>());
                GameObject.Destroy(pip);
            }
        }

        bird.transform.localPosition = Vector2.zero;
        bird.velocity = new Vector2(0, jumpValue);
        bird.angularVelocity = 0;
        bird.simulated = true;



        foreach (Rigidbody2D body in scrollers)
        {
            body.velocity = new Vector2(scrollV, 0f);
        }

        for (int i = 0; i < 5; i++)
        {
            doPipe((float) i);
        }

        foreach (GameObject pip in pipes)
        {
            pip.GetComponent<Rigidbody2D>().velocity += new Vector2(-2f, 0);
        }

         inGame = true;
    }

    void FixedUpdate()
    {
        if (inGame && pipes.Peek() != null)
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
    
    void doPipe(float i)
    {
        GameObject pip = Instantiate(pipe, gameObject.transform);
        pip.transform.localPosition = new Vector3(40f + i * 20f, Random.Range(-pipeBounds, pipeBounds), 0f);
        pipes.Enqueue(pip);
        scrollers.Add(pip.GetComponent<Rigidbody2D>());
        pip.GetComponent<Rigidbody2D>().velocity = new Vector2(scrollV, 0f);
    }

    void redoPipe()
    {
        float offset = 100f;
        GameObject first = pipes.Dequeue();
        first.transform.localPosition = new Vector2(first.transform.position.x + offset, Random.Range(-pipeBounds, pipeBounds));
        pipes.Enqueue(first);
    }

    public void gameOver()
    {
        inGame = false;

        foreach (Rigidbody2D obj in scrollers)
        {
            obj.velocity = Vector2.zero;
            bird.velocity = new Vector2(Random.Range(-10f, 10f), 30f);
            bird.angularVelocity = Random.Range(-180f, 180f);
        }
    }
}