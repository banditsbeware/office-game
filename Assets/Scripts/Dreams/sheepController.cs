using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tex

public class sheepController : MonoBehaviour
{
    [SerializeField] private GameObject sheepTemplate;
    [SerializeField] private  counter;
    private Queue<GameObject> sheepQueue = new Queue<GameObject>{};
    private int i = 120;

    // Start is called before the first frame update
    void Start()
    {
        spawnSheep();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            GameObject currentSheep = sheepQueue.Dequeue();
            currentSheep.GetComponent<sheep>().jump();
        }
    }

    private void FixedUpdate()
    {
        i -= 1;
        if (i <= 0)
        {
            i = 120;
            spawnSheep();
            Debug.Log("yup");
        }
        
    }

    public void spawnSheep()
    {
        sheepQueue.Enqueue(GameObject.Instantiate(sheepTemplate, transform));
    }



    
}
