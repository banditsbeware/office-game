using System.Collections;
using System.Collections.Generic;
using SpeakEasy;
using UnityEngine;

public class characterSwitcher : MonoBehaviour
{
    public GameObject characterA;
    public GameObject characterB;
    private interact_dialogue interactObject; //place on Station Object

    private GameObject activeCharacter;

    void Awake()
    {
        interactObject = gameObject.GetComponent<interact_dialogue>();

        if (Meta.Daily["afterWork"])
        {
            activeCharacter = characterA;
            characterB.SetActive(false);
        }
        else
        {
            activeCharacter = characterB;
            characterA.SetActive(false);
        }

        interactObject.characterObject = activeCharacter;
        interactObject.dialogue = activeCharacter.transform.Find("Dialogue").GetComponent<SEDialogue>();
    }
}
