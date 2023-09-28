using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager current;
    public delegate void OnItemCrafted();
    public OnItemCrafted onItemCrafted;

    private void Awake()
    {
        current = this;
    }

    public void ItemCrafted()
    {
        if(onItemCrafted != null)
        {
            onItemCrafted.Invoke();
        }
    }
}
