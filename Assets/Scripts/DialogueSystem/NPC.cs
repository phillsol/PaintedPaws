using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public RuntimeAnimatorController dialogueAnimator;
    bool foundPlayer;
    bool isTalking;

    private void Update()
    {
        if(foundPlayer && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void Interact()
    {
        if(isTalking == false)
        {
            PlayerController.current.FreezePlayer();
            DialogueManager.current.StartDialogue(this, dialogueAnimator);
            isTalking = true;
        }
        else
        {
            DialogueManager.current.NextDialogue();
        }
    }

    public void DialogueEnded()
    {
        PlayerController.current.UnFreezePlayer();
        isTalking = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            foundPlayer = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            foundPlayer = true;
        }
    }
}
