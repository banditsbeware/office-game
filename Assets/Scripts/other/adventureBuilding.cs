using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adventureBuilding : MonoBehaviour
{
    //entrance/exit mats and doors
    public bool onWelcomeMat;
    public bool onGoodbyeMat;
    public Vector2Int directionOfDoor;
    public MoveHim8bit playerMovement;

    //building and collider animation
    public GameObject interior;
    public GameObject interiorCollider;
    public GameObject exteriorCollider;

    private void Start() {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<MoveHim8bit>();
        interior = GameObject.FindGameObjectWithTag("interior");
        interiorCollider = GameObject.FindGameObjectWithTag("interiorCollider");
        exteriorCollider = GameObject.FindGameObjectWithTag("exteriorCollider");
        interior.SetActive(false);
    }

    private void FixedUpdate() {

        //when the player moves into a door, begin the proper building fades
        if (playerMovement.isNode && playerMovement.velocity == directionOfDoor && directionOfDoor != Vector2Int.zero)
        {
            Debug.Log("movement trigger!");
            if (onWelcomeMat)
            {
                StartCoroutine(FadeInInterior());
                return;
            }

            if (onGoodbyeMat)
            {
                StartCoroutine(FadeOutInterior());
                return;
            }
        }
    }

    private IEnumerator FadeOutInterior()
    {
        Debug.Log("Started Fade out");

        interiorCollider.SetActive(false);
        playerMovement.skipNode = true;

        yield return new WaitForSeconds(.3f);

        interior.SetActive(false);
        exteriorCollider.SetActive(true);
        

        Debug.Log("Ended Fade Out");
        
    }

    private IEnumerator FadeInInterior()
    {
        Debug.Log("ee");

        exteriorCollider.SetActive(false);
        playerMovement.skipNode = true;

        yield return new WaitForSeconds(.3f);

        interior.SetActive(true);
        interiorCollider.SetActive(true);
        
    }
}

