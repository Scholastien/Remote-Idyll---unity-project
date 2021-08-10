using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonMonobehaviour<InventoryManager>
{
    private Dictionary<string, ItemDetails> itemDetailsDictionary;
    private string[] selectedInventoryItem;
    public List<InventoryItem>[] inventoryLists;
    [HideInInspector] public int[] inventoryListCapacityInArray;

    [SerializeField] private SO_ItemList itemList = null;

    protected override void Awake()
    {
        base.Awake();

        CreateInventoryList();

        CreateItemDetailsDictionary();

        InitializeSelectedInventoryItem();
    }

    private void InitializeSelectedInventoryItem()
    {
        selectedInventoryItem = new string[(int)InventoryLocation.count];

        for (int i = 0; i < selectedInventoryItem.Length; i++)
        {
            selectedInventoryItem[i] = "";
        }
    }

    private void CreateInventoryList()
    {
        inventoryLists = new List<InventoryItem>[(int)InventoryLocation.count];

        for (int i = 0; i < (int)InventoryLocation.count; i++)
        {
            inventoryLists[i] = new List<InventoryItem>();
        }

        inventoryListCapacityInArray = new int[(int)InventoryLocation.count];
        inventoryListCapacityInArray[(int)InventoryLocation.player] = Settings.playerInitialInventoryCapacity;
    }

    private void CreateItemDetailsDictionary()
    {
        itemDetailsDictionary = new Dictionary<string, ItemDetails>();

        foreach (var item in itemList.itemDetails)
        {
            itemDetailsDictionary.Add(item.itemCode, item);
        }
    }

    private void AddItemAtPosition(List<InventoryItem> inventoryList, string itemCode)
    {
        InventoryItem inventoryItem = new InventoryItem(itemCode, 1);
        inventoryList.Add(inventoryItem);

        //Debug.ClearDeveloperConsole();
        //DebugPrintInventoryList(inventoryList);
    }

    private void AddItemAtPosition(List<InventoryItem> inventoryList, string itemCode, int position)
    {
        int quantity = inventoryList[position].itemQuantity + 1;
        InventoryItem inventoryItem = new InventoryItem(itemCode, quantity);
        inventoryList[position] = inventoryItem;

        //Debug.ClearDeveloperConsole();
        //DebugPrintInventoryList(inventoryList);
    }


    public void AddItem(InventoryLocation inventoryLocation, Item item)
    {
        string itemCode = item.ItemCode;
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];

        int itemPosition = FindItemInInventory(inventoryLocation, itemCode);

        if (itemPosition != -1)
        {
            AddItemAtPosition(inventoryList, itemCode, itemPosition);
        }
        else
        {
            AddItemAtPosition(inventoryList, itemCode);
        }
        EventHandler.CallInventoryUpdateEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
    }


    public void AddItem(InventoryLocation inventoryLocation, Item item, GameObject gameObjectToDelete)
    {
        AddItem(inventoryLocation, item);

        //TODO: Animation on picked up Items.

        Destroy(gameObjectToDelete);
    }


    private void RemoveItemAtPosition(List<InventoryItem> inventoryList, string itemCode, int itemPosition)
    {
        InventoryItem inventoryItem = new InventoryItem();
        int quantity = inventoryList[itemPosition].itemQuantity - 1;

        if (quantity > 0)
        {
            inventoryItem.itemQuantity = quantity;
            inventoryItem.itemCode = itemCode;
            inventoryList[itemPosition] = inventoryItem;
        }
        else
        {
            inventoryList.RemoveAt(itemPosition);
        }
    }

    public void RemoveItem(InventoryLocation inventoryLocation, string itemCode)
    {
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];

        int itemPosition = FindItemInInventory(inventoryLocation, itemCode);

        if (itemPosition != -1)
        {
            RemoveItemAtPosition(inventoryList, itemCode, itemPosition);
        }
        EventHandler.CallInventoryUpdateEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
    }



    public void SwapInventoryItems(InventoryLocation inventoryLocation, int fromItem, int toItem)
    {
        if (fromItem < inventoryLists[(int)inventoryLocation].Count && toItem < inventoryLists[(int)inventoryLocation].Count && fromItem != toItem && fromItem >= 0 && toItem >= 0)
        {
            InventoryItem fromInventoryItem = inventoryLists[(int)inventoryLocation][fromItem];
            InventoryItem toInventoryItem = inventoryLists[(int)inventoryLocation][toItem];

            inventoryLists[(int)inventoryLocation][toItem] = fromInventoryItem;
            inventoryLists[(int)inventoryLocation][fromItem] = toInventoryItem;


            EventHandler.CallInventoryUpdateEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
        }
    }


    public int FindItemInInventory(InventoryLocation inventoryLocation, string itemCode)
    {
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];
        for (int i = 0; i < inventoryList.Count; i++)
        {
            if (inventoryList[i].itemCode == itemCode)
            {
                return i;
            }
        }
        return -1;
    }

    public ItemDetails GetItemDetails(string itemCode)
    {
        ItemDetails itemDetails = ScriptableObject.CreateInstance<ItemDetails>();


        if (itemDetailsDictionary.TryGetValue(itemCode, out itemDetails))
        {
            Console.WriteLine("For key = " + itemCode + ", value = {0}.", itemDetails);
            return itemDetails;
        }
        else
        {
            throw new Exception("Cannot be found with the code: " + itemCode);
        }
    }

    public string GetItemTypeDescription(ItemType itemType)
    {
        string itemTypeDescription;
        switch (itemType)
        {
            case ItemType.Watering_tool:
                itemTypeDescription = Settings.WateringTool;
                break;
            case ItemType.Hoeing_tool:
                itemTypeDescription = Settings.HoeingTool;
                break;
            case ItemType.Chopping_tool:
                itemTypeDescription = Settings.ChoppingTool;
                break;
            case ItemType.Breaking_tool:
                itemTypeDescription = Settings.BreakingTool;
                break;
            case ItemType.Reaping_tool:
                itemTypeDescription = Settings.ReapingTool;
                break;
            case ItemType.Collecting_tool:
                itemTypeDescription = Settings.CollectingTool;
                break;
            default:
                itemTypeDescription = itemType.ToString();
                break;
        }
        return itemTypeDescription;
    }


    public void SetSelectedInventoryItem(InventoryLocation inventoryLocation, string itemCode)
    {
        selectedInventoryItem[(int)inventoryLocation] = itemCode;
    }

    public void ClearSelectedInventoryItem(InventoryLocation inventoryLocation)
    {
        selectedInventoryItem[(int)inventoryLocation] = "";
    }

    // private void DebugPrintInventoryList(List<InventoryItem> inventoryList)
    // {
    //     foreach (InventoryItem i in inventoryList)
    //     {
    //         Debug.Log("Item Description: " + InventoryManager.Instance.GetItemDetails(i.itemCode).itemDescription + "\tItemQuantity: " + i.itemQuantity);
    //     }
    //     Debug.Log("*****************************************************************************");
    // }
}
