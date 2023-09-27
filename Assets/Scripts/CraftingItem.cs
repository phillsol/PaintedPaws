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
        canCraft = true;

        Debug.Log("craft inven");

        craftingImage.sprite = craftingItem.itemSprite;
        if (!IslandInventory.current.CheckForRecipe(craftingItem))
        {
            craftingImage.color = Color.black;
            canCraft = false;
        }

        int i = 0;
        foreach(IslandItemCraftStats item in craftingItem.itemNeeded)
        {
            Debug.Log("Craft item active");
            itemList[i].gameObject.SetActive(true);

            print(itemList[i]);
            print(itemList[i].sprite);

            print(item);
            print(item.item);
            print(item.item.itemSprite);



            itemList[i].sprite = item.item.itemSprite;
            if (IslandInventory.current.CheckForItemInInventory(item.item))
            {
                //We have it
                itemList[i].color = Color.white;
            }
            else
            {
                canCraft = false;
            }
            i++;
        }
    }

    public void Craft()
    {
        Debug.Log("Test");
        
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
        }
        else
        {
            //Stack too full
            Debug.Log("Full?");
        }
    }
}
