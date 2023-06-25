using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goodbyeMat : MonoBehaviour
{
    [SerializeField] private Vector2Int dir = new Vector2Int(0, -1);
    private adventureBuilding building;
    void Start()
    {
        building = transform.parent.parent.GetComponent<adventureBuilding>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Entered  Goodbye");
        building.onGoodbyeMat = true;
        building.directionOfDoor = dir;
    }

    private void OnTriggerExit2D(Collider2D other) {
        building.onGoodbyeMat = false;
        Debug.Log("Exited Goodbye");
    }
}

