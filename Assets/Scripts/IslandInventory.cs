using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandInventory : MonoBehaviour
{

    public static IslandInventory current;
    public List<IslandItem> craftableRecipes = new List<IslandItem>();
    public List<IslandItemInventoryStats> inventory = new List<IslandItemInventoryStats>();

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
        
    }
    public bool CheckForRecipe(IslandItem item)
    {
        foreach (IslandItem i in craftableRecipes)
        {
            if(i == item)
            {
                return true;
            }
        }
        return false;
    }
    public void ItemCrush(IslandItem i)
    {
        if (ItemCollected(i.crushedItem))
        {
            RemoveItem(i);
        }
        else
        {
            //Too full
            Debug.Log("Inventory full");
        }
    }
    public bool ItemCollected(IslandItem itemPickedUp)
    {
        Debug.Log(itemPickedUp.itemSprite);
        foreach(IslandItemInventoryStats i in inventory)
        {
            if (i.item == itemPickedUp)
            {
                if (i.amount < i.item.itemStackLimit)
                {
                    //Can add item
                    Debug.Log("item +1");
                    i.amount++;
                    return true;
                }
                else
                {
                    Debug.Log("Stack full");
                    //at stack limit
                    return false;
                }
            }
        }

        Debug.Log("Add Item");
        IslandItemInventoryStats newItem = new IslandItemInventoryStats();
        newItem.item = itemPickedUp;
        newItem.amount = 1;
        inventory.Add(newItem);
        return true;
    }
    public bool CheckForItemInInventory(IslandItem item)
    {
        foreach(IslandItemInventoryStats i in inventory)
        {
            if(i.item == item)
            {
                return true;
            }
        }
        return false;
    }
    public bool CheckItemForQuantity(IslandItem item, int amount)
    {
        foreach (IslandItemInventoryStats i in inventory)
        {
            if (i.item == item)
            {
                if (amount >= i.amount)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }
    public void RemoveItem(IslandItem item)
    {
        foreach (IslandItemInventoryStats i in inventory)
        {
            if (i.item == item)
            {
                i.amount -= 1;
                if(i.amount <= 0)
                {
                    inventory.Remove(i);
                    return;
                }
            }
        }
    }
}

[System.Serializable]
public class IslandItemInventoryStats
{
    public IslandItem item;
    public int amount;
}
