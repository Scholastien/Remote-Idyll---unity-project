[System.Serializable]
public struct InventoryItem
{
    public string itemCode;
    public int itemQuantity;

    public InventoryItem(string itemCode, int itemQuantity)
    {
        this.itemCode = itemCode;
        this.itemQuantity = itemQuantity;
    }
}
