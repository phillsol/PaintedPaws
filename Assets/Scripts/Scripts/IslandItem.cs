using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Item", menuName = "new Item")]
public class IslandItem : ScriptableObject
{
    public bool isQuestItem;
    public string itemName;
    [TextArea]
    public string itemDescription;
    public int itemStackLimit;
    public Sprite itemItem;
    public GameObject itemPrefab;
    public IslandItem crushedItem;
    [NonReorderable]
    public List<IslandItemCraftStats> itemNeeded = new List<IslandItemCraftStats>();
}

[System.Serializable]
public class IslandItemCraftStats
{
    public IslandItem item;
    public int itemAmount;
}
