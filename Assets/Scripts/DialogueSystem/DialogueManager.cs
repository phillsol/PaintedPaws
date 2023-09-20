using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    public static DialogueManager current;
    Animator anim;
    NPC currentNPC;

    public GameObject dialogueObject;
    public TextMeshProUGUI dialogueText;

    // Start is called before the first frame update
    void Start()
    {
        current = this;
        anim = GetComponent<Animator>();
    }

    public void StartDialogue(NPC npc, RuntimeAnimatorController dialogueAnimator)
    {
        Debug.Log("Start");
        currentNPC = npc;
        anim.runtimeAnimatorController = dialogueAnimator;
        anim.enabled = true;
    }
    public void NextDialogue()
    {
        anim.SetBool("NextDialogue", true);
    }
    public void NextNode(DialogueNode.DialogueType dt, string d, List<string> choices)
    {
        switch (dt)
        {
            case DialogueNode.DialogueType.dialogue:
                ShowDialogue(d);
                break;
            case DialogueNode.DialogueType.choices:

                break;
            case DialogueNode.DialogueType.end:
                EndDialogue();
                break;
        }
    }

    public void ShowDialogue(string d)
    {
        dialogueObject.SetActive(true);
        dialogueText.text = d;
    }

    void EndDialogue()
    {
        dialogueObject.SetActive(false);
        currentNPC.DialogueEnded();
        anim.enabled = false;
        anim.runtimeAnimatorController = null;
    }
}
