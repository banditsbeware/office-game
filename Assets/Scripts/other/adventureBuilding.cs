using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class adventureBuilding : MonoBehaviour
{
    //entrance/exit mats and doors
    [HideInInspector] public bool onWelcomeMat;
    [HideInInspector] public bool onGoodbyeMat;
    public Vector2Int directionOfDoor;
    public MoveHim8bit playerMovement;

    //building and collider animation
    private GameObject interior;
    private GameObject interiorCollider;
    private GameObject exterior;
    private GameObject exteriorCollider;
    private GameObject buildingEntrance; 
    public static List<GameObject> allExteriorsAndEntrances; // total collection among all entrances/exteriors that have collider components in map
    private List<GameObject> otherExteriorsAndEntrances; // above, minus those belonging to this instance of a building ()

    private void Start() {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<MoveHim8bit>();
        interior = transform.Find("interior").gameObject;
        interiorCollider = interior.transform.Find("interiorCollider").gameObject;
        exterior = transform.Find("exterior").gameObject;
        exteriorCollider = exterior.transform.Find("exteriorCollider").gameObject;
        buildingEntrance = transform.Find("entrance").gameObject;

        if(allExteriorsAndEntrances == null) 
        {
            allExteriorsAndEntrances = GameObject.FindGameObjectsWithTag("exteriorCollider").ToList<GameObject>();
        }

        otherExteriorsAndEntrances = allExteriorsAndEntrances.GetRange(0, allExteriorsAndEntrances.Count);
        otherExteriorsAndEntrances.Remove(exterior);
        otherExteriorsAndEntrances.Remove(buildingEntrance);

        interior.SetActive(false);
    }

    private void FixedUpdate() {
        //when the player moves into a door, begin the proper building fades
        if (playerMovement.isNode && playerMovement.velocity == directionOfDoor && directionOfDoor != Vector2Int.zero)
        {
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

    private IEnumerator FadeInInterior()
    {
        exteriorCollider.SetActive(false);
        playerMovement.skipNode = true;

        yield return new WaitForSeconds((float) playerMovement.speed * 1.5f / 50f);  // speed/50 is the amount of time it will take to travel one grid tile, so * 1.5 will trigger the animations halfway between the entrance and the next grid node. 

        interior.SetActive(true);
        interiorCollider.SetActive(true);
        foreach (GameObject externalObj in otherExteriorsAndEntrances)
        {
            externalObj.SetActive(false);
        }
    }

    private IEnumerator FadeOutInterior()
    {
        interiorCollider.SetActive(false);
        playerMovement.skipNode = true;

        yield return new WaitForSeconds((float) playerMovement.speed * 1.1f / 50f);

        interior.SetActive(false);
        exteriorCollider.SetActive(true);
        foreach (GameObject externalObj in otherExteriorsAndEntrances)
        {
            externalObj.SetActive(true);
        }
    }

    
}

