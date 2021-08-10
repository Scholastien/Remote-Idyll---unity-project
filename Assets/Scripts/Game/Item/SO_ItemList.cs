
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "so_ItemList", menuName = "Scriptable Object/Item/Item List")]
public class SO_ItemList : ScriptableObject
{
    //[SerializeField]
    public List<ItemDetails> itemDetails;
    public List<ItemDetails> GetItemWithCode(string code)
    {
        return itemDetails.FindAll(i => i.itemCode == code);
    }

    public void GetAllOccurrence(List<ItemDetails> ItemList)
    {
        List<string> occ = ListExtention.GetOccurrenceList<string>(GetItemsCode(ItemList));

        if (occ.Count != 0)
        {
            string message = "";
            List<ItemDetails> result = new List<ItemDetails>();

            foreach (var i in occ)
            {
                result.AddRange(ItemList.FindAll(x => x.itemCode == i));
            }

            foreach (var i in result)
            {
                message = String.Join("\n", message, "The item named " + /*AssetDatabase.GetAssetPath(i)*/ i.name + " needs to have a different itemcode");
            }

            throw new ResourceItemException(occ, String.Join(", ", occ) + "\n" + message);
        }
    }

    private List<string> GetItemsCode(List<ItemDetails> ItemList)
    {
        List<string> detailCodes = new List<string>();

        foreach (var item in ItemList)
        {
            detailCodes.Add(item.itemCode);
        }

        return detailCodes;
    }
}