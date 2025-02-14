using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class sheepController : MonoBehaviour
{
    [SerializeField] private GameObject[] sheepTemplates;
    [SerializeField] private TMP_Text counter;
    private Queue<GameObject> sheepQueue = new Queue<GameObject>{};
     GameObject currentSheep = null;
    private int sheepCount = 0;
    private int i = 100;
    public bool isBurning = false;

    [SerializeField] private int spawnTimer = 240;

    // Start is called before the first frame update
    void Start()
    {
        spawnSheep();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")) //jump sheep
        {           
            if (sheepQueue.Count != 0)
            {
                currentSheep = sheepQueue.Dequeue();
            }
            if (currentSheep != null)
            {
                currentSheep.GetComponent<sheep>().jump();
            }
        }
    }

    private void FixedUpdate()
    {
        i -= 1;
        if (i <= 0)
        {
            i = spawnTimer;
            spawnSheep();
        }
        
    }

    public void spawnSheep()
    {
        GameObject s = Instantiate(sheepTemplates[Random.Range(0, sheepTemplates.Length)], transform);
        sheepQueue.Enqueue(s);
        if (isBurning)
        {
            s.GetComponent<Animator>().SetBool("burn", true);
        }
       
    }

    public void sheepCleared()
    {
        sheepCount++;
        counter.text = sheepCount.ToString();
    }
    public void sheepHit()
    {
        isBurning = true;

        for (int i=0; i <= sheepQueue.Count; i++)
        {
            sheepQueue.Dequeue().GetComponent<Animator>().SetBool("burn", true);
        }
    }
}
