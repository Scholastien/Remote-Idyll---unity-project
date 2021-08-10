using System;
using UnityEngine;
using Sirenix.OdinInspector;


[System.Serializable, CreateAssetMenu(fileName = "so_ItemDetail", menuName = "Scriptable Object/Item/New Item")]
public class ItemDetails : ScriptableObject
{
    [SerializeField, HideInInspector]
    private SerializableGuid _uniqueIdentifier; // identifier for the a specific item
    [ShowInInspector, ReadOnly]
    public string itemCode { get => _uniqueIdentifier.Value; set { _uniqueIdentifier.Value = value; } } //should be unique
    public ItemType itemType;
    public string itemDescription;
    public Sprite itemSprite;
    public string itemLongDescription;
    public short itemUseGridRadius; //range of a tool based on the grid
    public float itemUseRadius; // range of a tool outside of the grid: props placed outside of the Tilemap grid (example: scenery object)
    public bool isStartingItem;
    public bool canBePicked;
    public bool canBeDropped;
    public bool canBeEaten;
    public bool canBeCarried;

    public void AssignGUID()
    {
        Debug.Log("AssignGUID");
        _uniqueIdentifier = Guid.NewGuid();
    }

    [Button("Copy Item Code")]
    public void CopyToClipboard()
    {
        TextEditor te = new TextEditor();
        te.text = itemCode;
        te.SelectAll();
        te.Copy();
    }
}