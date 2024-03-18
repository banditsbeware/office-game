using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class sheepController : MonoBehaviour
{
    [SerializeField] private GameObject sheepTemplate;
    [SerializeField] private TMP_Text counter;
    private Queue<GameObject> sheepQueue = new Queue<GameObject>{};
    private int sheepCount = 0;
    private int i = 100;

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
            GameObject currentSheep = sheepQueue.Dequeue();
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
            i = 240;
            spawnSheep();
        }
        
    }

    public void spawnSheep()
    {
        sheepQueue.Enqueue(GameObject.Instantiate(sheepTemplate, transform));   
    }

    public void sheepCleared()
    {
        sheepCount++;
        counter.text = sheepCount.ToString();
    }  
}
