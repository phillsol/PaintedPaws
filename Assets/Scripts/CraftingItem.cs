using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingItem : MonoBehaviour
{
    bool canCraft = true;
    public IslandItem craftingItem;
    public Image craftingImage;
    public List<Image> itemList = new List<Image>();

    private void OnEnable()
    {
        ResetVisuals();
        CraftingManager.current.onItemCrafted += ResetVisuals;
    }

    private void OnDisable()
    {
        CraftingManager.current.onItemCrafted -= ResetVisuals;
    }

    public void ResetVisuals()
    {
        craftingImage.sprite = craftingItem.itemItem;
        if (!IslandInventory.current.CheckForRecipe(craftingItem))
        {
            craftingImage.color = Color.black;
            canCraft = false;
        }

        int i = 0;
        canCraft = true;
        foreach (IslandItemCraftStats item in craftingItem.itemNeeded)
        {
            itemList[i].gameObject.SetActive(true);
            itemList[i].sprite = item.item.itemItem;
            if (IslandInventory.current.CheckForItemInInventory(item.item))
            {
                //We have it
                itemList[i].color = Color.white;
            }
            else
            {
                itemList[i].color = Color.black;
                canCraft = false;
            }
            i++;
        }
    }

    public void Craft()
    {
        if (!canCraft)
        {
            Debug.Log("Can't Craft");
            //Can't craft
            return;
        }

        bool addCraft = IslandInventory.current.ItemCollected(craftingItem);
        if (addCraft)
        {
            foreach (IslandItemCraftStats item in craftingItem.itemNeeded)
            {
                IslandInventory.current.RemoveItem(item.item);
                Debug.Log("Remove Item");
            }
            Debug.Log("Crafted");
            CraftingManager.current.ItemCrafted();
        }
        else
        {
            //Stack too full
            Debug.Log("Full?");
        }
    }
}
