using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandItemPickup : MonoBehaviour
{
    public IslandItem item;
    public float itemCooldown = 60;
    bool playerFound;
    bool itemCollected;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerFound && itemCollected == false)
        {
            Interact();
        }
    }

    void Interact()
    {
        if (IslandInventory.current.ItemCollected(item))
        {
            transform.GetChild(0).gameObject.SetActive(false);
            itemCollected = true;
            Invoke("Respawn", itemCooldown);
        }
    }

    void Respawn()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        itemCollected = false;
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
