using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MasayaScripts.Quest;

namespace MasayaScripts
{
    [System.Serializable]
    public class NPC : MonoBehaviour
    {
        public RuntimeAnimatorController dialogueController; //Single Dialogue
        //Multiple Dialogues
        public List<DialogueConditions> dialogueConditions = new List<DialogueConditions>();
        public UnityEvent enterEvent;
        public UnityEvent exitEvent;
        public UnityEvent finishedDialogueEvent;
        bool usingCondition;
        int conditionIndex;
        bool playerFound;
        bool isTalking;

        private void Update()
        {
            if (playerFound == true & Input.GetKeyDown(KeyCode.E))
            {
                if (isTalking == true)
                {
                    DialogueManagerV2.current.NextDialogue();
                    return;
                }

                if (dialogueController == null)
                {
                    foreach (DialogueConditions checkDialogueCondition in dialogueConditions)
                    {
                        bool conditionMet = CheckCondition(checkDialogueCondition);
                        if (conditionMet)
                        {
                            if (checkDialogueCondition.dialogueController != null)
                            {
                                isTalking = true;
                                PlayerController.current.FreezePlayer();
                                exitEvent.Invoke();
                                usingCondition = true;
                                conditionIndex = dialogueConditions.IndexOf(checkDialogueCondition);
                                DialogueManagerV2.current.StartDialogue(this, checkDialogueCondition.dialogueController);
                                return;
                            }
                            else
                            {
                                checkDialogueCondition.finishedDialogueEvent.Invoke();
                                return;
                            }
                        }
                    }

                    exitEvent.Invoke();
                    FinishedDialogue();
                    return;
                }


                isTalking = true;
                PlayerController.current.FreezePlayer();
                exitEvent.Invoke();
                DialogueManagerV2.current.StartDialogue(this, dialogueController);
            }
        }

        bool CheckCondition(DialogueConditions checkDialogueCondition)
        {
            switch (checkDialogueCondition.dialogueConditionType)
            {
                case DialogueConditions.DialogueConditionType.QuestComplete:
                    if (QuestManager.current.CheckQuestStatus(checkDialogueCondition.questName))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case DialogueConditions.DialogueConditionType.QuestIncomplete:
                    if (!QuestManager.current.PlayerHasQuest(checkDialogueCondition.questName))
                    {
                        return false;
                    }

                    if (QuestManager.current.CheckQuestStatus(checkDialogueCondition.questName))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case DialogueConditions.DialogueConditionType.QuestToGive:
                    if (QuestManager.current.PlayerHasQuest(checkDialogueCondition.questName))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case DialogueConditions.DialogueConditionType.HasItem:
                    if (checkDialogueCondition.itemAmountRequired > 0)
                    {
                        if (IslandInventory.current.CheckItemForQuantity(checkDialogueCondition.itemRequired, checkDialogueCondition.itemAmountRequired))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (IslandInventory.current.CheckForItemInInventory(checkDialogueCondition.itemRequired))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                case DialogueConditions.DialogueConditionType.NoItem:
                    if (IslandInventory.current.CheckForItemInInventory(checkDialogueCondition.itemRequired))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case DialogueConditions.DialogueConditionType.Default:
                    return true;
            }
            return false;
        }

        public void FinishedDialogue()
        {
            PlayerController.current.UnFreezePlayer();
            isTalking = false;
            finishedDialogueEvent.Invoke();
            if (usingCondition)
            {
                dialogueConditions[conditionIndex].finishedDialogueEvent.Invoke();
            }
        }
        public void AddQuest(string questName)
        {
            QuestManager.current.AddQuest(questName);
        }
        public void CompleteQuest(string questName)
        {
            QuestManager.current.HasCompletedQuest(questName);
        }
        public void OnTriggerEnter(Collider other)
        {
            enterEvent.Invoke();
            playerFound = true;
        }

        public void OnTriggerExit(Collider other)
        {
            exitEvent.Invoke();
            playerFound = false;
            isTalking = false;
        }
        public void AddItem(IslandItem item)
        {
            IslandInventory.current.ItemCollected(item);
        }
        public void AddRecipe(IslandItem item)
        {
            IslandInventory.current.AddRecipe(item);
        }
        public void RemoveItem(IslandItem item)
        {
            IslandInventory.current.RemoveItem(item);
        }
    }
}

[System.Serializable]
public class DialogueConditions
{
    public DialogueConditionType dialogueConditionType;
    public RuntimeAnimatorController dialogueController;
    public string questName;
    public IslandItem itemRequired;
    public int itemAmountRequired;
    public UnityEvent finishedDialogueEvent;

    public enum DialogueConditionType
    {
        QuestComplete,
        QuestIncomplete,
        QuestToGive,
        HasItem,
        NoItem,
        Default
    }
}