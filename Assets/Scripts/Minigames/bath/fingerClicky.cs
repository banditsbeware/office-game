using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class fingerClicky : MonoBehaviour
{
    [SerializeField] private Collider2D interactor;
    private Animator toClick;
    private bool interactable = false;
    [System.NonSerialized] public bool clicked = false;

    void Start()
    {
        toClick = transform.GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D col) //can only click when fingy on something clickable
    {
        if (col == interactor)
        {
            interactable = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col == interactor)
        {
            interactable = false;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && interactable)
        {
            clicked = true;
            toClick.SetTrigger("clicked");
        }
        else
        {
            clicked = false;
        }
    }

}
