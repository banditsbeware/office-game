using System.Collections;
using System.Collections.Generic;
using SpeakEasy;
using UnityEngine;

public class characterSwitcher : MonoBehaviour
{
    public GameObject characterA;
    public GameObject characterB;
    public GameObject characterASprite;
    public GameObject characterBSprite;
    private interact_dialogue interactObject; //place on Station Object

    private GameObject activeCharacter;

    void Awake()
    {
        interactObject = gameObject.GetComponent<interact_dialogue>();

        if (Meta.Daily["afterWork"])
        {
            activeCharacter = characterA;
            characterBSprite.SetActive(false);
            characterB.SetActive(false);
        }
        else
        {
            activeCharacter = characterB;
            characterASprite.SetActive(false);
            characterA.SetActive(false);
        }

        interactObject.characterObject = activeCharacter;
        interactObject.dialogue = activeCharacter.transform.Find("Dialogue").GetComponent<SEDialogue>();


    }
}
