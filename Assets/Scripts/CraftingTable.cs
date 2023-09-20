using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : MonoBehaviour
{
    bool playerFound;
    bool bookOpen;
    public GameObject craftingMenu;

    private void Update()
    {
        if(playerFound && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void Interact()
    {
        PlayerController.current.FreezePlayer();
        craftingMenu.SetActive(true);
    }

    public void CloseCraftMenu()
    {
        PlayerController.current.UnFreezePlayer();
        craftingMenu.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerFound = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerFound = false;
        }
    }
}
