
using UnityEngine;

public class Item : MonoBehaviour
{
    [ItemCodeDescription]
    [SerializeField]
    private string _itemCode;
    private SpriteRenderer spriteRenderer;
    public string ItemCode { get { return _itemCode; } set { _itemCode = value; } }

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        if (!string.IsNullOrEmpty(ItemCode))
        {
            Init(ItemCode);
        }
    }

    public void Init(string itemCodeParam)
    {
        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCodeParam);

        spriteRenderer.sprite = itemDetails.itemSprite;

        if (itemDetails.itemType == ItemType.Reapable_scenery)
        {
            gameObject.AddComponent<ItemNudge>();
        }
    }
}