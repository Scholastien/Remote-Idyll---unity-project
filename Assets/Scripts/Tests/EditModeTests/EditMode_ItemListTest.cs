using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

public class EditMode_ItemListTest
{


    [Test]
    public void IsItemListUniqueInResources()
    {
        SO_ItemList[] allItemListInResources = Resources.LoadAll<SO_ItemList>("Scriptable Object");
        Assert.IsFalse(allItemListInResources.Length > 1, "More than one ItemList have been found in the asset folder");
        Assert.IsFalse(allItemListInResources.Length == 0, "None ItemList have been found in the asset folder");
    }

    [Test]
    public void IsItemsCodeUniqueInResources()
    {
        List<ItemDetails> ItemList = new List<ItemDetails>(Resources.LoadAll<ItemDetails>("Scriptable Object"));
        List<string> indexes = new List<string>();

        foreach (var item in ItemList)
        {
            indexes.Add(item.itemCode);
        }
        List<string> occ = ListExtention.GetOccurrenceList<string>(indexes);

        Assert.True(occ.Count == 0, "Cannot Find Item with Id Code");
    }

    [Test]
    public void GetAllOccurrence_DuplicatedIndexInItemList_ShouldThrowException()
    {
        SO_ItemList ItemList = ScriptableObject.CreateInstance<SO_ItemList>();
        ItemList.itemDetails = new List<ItemDetails>();
        string itemCode = "test";

        for (int i = 0; i < 2; i++)
        {
            ItemList.itemDetails.Add(ScriptableObject.CreateInstance<ItemDetails>());
            ItemList.itemDetails[i].itemCode = itemCode;
        }
        var ex = Assert.Throws<ResourceItemException>(() => ItemList.GetAllOccurrence(ItemList.itemDetails));

        Assert.That(ex.listOfOccurrence, Is.EqualTo(new List<string> { itemCode }));
    }

    [Test]
    public void GetAllOccurrence_UniqueIndexInItemList_ShouldNotThrowException()
    {
        SO_ItemList ItemList = ScriptableObject.CreateInstance<SO_ItemList>();
        ItemList.itemDetails = new List<ItemDetails>();
        string itemCode = "test";

        for (int i = 0; i < 3; i++)
        {
            ItemList.itemDetails.Add(ScriptableObject.CreateInstance<ItemDetails>());
            ItemList.itemDetails[i].itemCode = itemCode + i;
        }

        Assert.DoesNotThrow(() => ItemList.GetAllOccurrence(ItemList.itemDetails), "an error occurred in the list");
    }

    [Test]
    public void GetItemWithCode_AddingThreeNewUniqueItem_ExpectFindingIt()
    {
        SO_ItemList ItemList = ScriptableObject.CreateInstance<SO_ItemList>();
        ItemList.itemDetails = new List<ItemDetails>();
        string itemCode = "Test correct";

        for (int i = 0; i < 2; i++)
        {
            ItemList.itemDetails.Add(ScriptableObject.CreateInstance<ItemDetails>());
            ItemList.itemDetails[i].itemCode = "Test invalid";
        }
        ItemList.itemDetails[1].itemCode = itemCode;
        List<ItemDetails> result = ItemList.GetItemWithCode(itemCode);
        List<ItemDetails> expectedResult = new List<ItemDetails> { ItemList.itemDetails[1] };

        Assert.AreEqual(expectedResult, result, "Cannot Find Item with Id Code");
    }

}
