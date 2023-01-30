using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interact_square : interact
{
    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isGame)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                sprite.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            }
        }
    }
}
