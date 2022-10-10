using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//in progrss script for chaos-ing the desks
public class CubsComponents : MonoBehaviour
{
    [SerializeField] private Sprite[] monitors;
    public float fWRange;
    private float lapYAdjust = .2f;

    void Start()
    {
        foreach (Transform child in transform) 
        {
            //randomize monitor on each desk
            Transform comp = child.Find("Cubicle_Computer");
            SpriteRenderer rend = comp.GetComponent<SpriteRenderer>();
            rend.sprite = monitors[Random.Range(0, monitors.Length)];


            Transform boart = child.Find("Cubicle_Keyboart");
            Transform mouse = child.Find("Cubicle_Mouse");

            boart.position = new Vector3(boart.position.x + Random.Range(-fWRange, fWRange), boart.position.y, boart.position.z);
            mouse.position = new Vector3(mouse.position.x + Random.Range(-fWRange, fWRange), mouse.position.y + Random.Range(-fWRange, fWRange), boart.position.z);

            //remove mouse and keyboard from laptop desks
            if (rend.sprite.name == "Cubicle_Laptop")
            {
                comp.position = new Vector3(comp.position.x, comp.position.y - lapYAdjust, comp.position.z);
                Destroy(boart.gameObject);
                Destroy(mouse.gameObject);
            }
        }
    }

}
