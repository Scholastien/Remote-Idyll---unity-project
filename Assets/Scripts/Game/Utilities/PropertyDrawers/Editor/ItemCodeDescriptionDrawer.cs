using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ItemCodeDescriptionAttribute))]
public class ItemCodeDescriptionDrawer : PropertyDrawer
{
    //int tSize = 64;
    Texture2D texture;
    bool importFromAssetFolder;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Change the returned property height to the double to cater for the additional item code description
        return EditorGUI.GetPropertyHeight(property) * 4;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that prefab override logic works on the entire property
        EditorGUI.BeginProperty(position, label, property);



        if (property.propertyType == SerializedPropertyType.String)
        {
            EditorGUI.BeginChangeCheck(); // start check for changed values

            // Draw item code
            var newValue = EditorGUI.TextField(new Rect(position.x, position.y, position.width, position.height / 4), label, property.stringValue);

            // Draw item description
            EditorGUI.LabelField(new Rect(position.x, position.y + position.height / 4, position.width, position.height / 4), "Item Description:", GetItemDescription(property.stringValue));


            Rect pos = new Rect(position.x + position.width / 2, position.y + position.height / 2, position.height / 2, position.height / 2);

            GUIStyle style = new GUIStyle();
            style.normal.background = texture;
            EditorGUI.LabelField(pos, GUIContent.none, style);

            pos = new Rect(position.x, position.y + position.height / 2, position.height / 2, position.height / 2);

            importFromAssetFolder = EditorGUI.Foldout(pos, importFromAssetFolder, "Import From Asset Folder", true);

            if (importFromAssetFolder)
            {
                pos = new Rect(position.x, position.y - 4 + position.height / 2, position.height / 2, position.height / 2);
                ItemDetails itemDetails = null;
                itemDetails = (ItemDetails)EditorGUILayout.ObjectField(itemDetails, typeof(ItemDetails), false);
                if (itemDetails != null)
                {
                    newValue = itemDetails.itemCode;
                    property.stringValue = newValue;
                }
            }

            // if item code value has changed, set the value to new value
            if (EditorGUI.EndChangeCheck())
            {
                property.stringValue = newValue;
            }
        }






        EditorGUI.EndProperty();

    }


    private Texture2D textureFromSprite(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, TextureFormat.RGB24, false);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
        {
            return sprite.texture;
        }
    }


    public string GetItemDescription(string itemCode)
    {
        SO_ItemList[] itemList = Resources.LoadAll<SO_ItemList>("Scriptable Object");

        List<ItemDetails> itemDetailsList = new List<ItemDetails>();

        foreach (var i in itemList)
        {
            itemDetailsList.AddRange(i.itemDetails);
        }

        ItemDetails itemDetail = itemDetailsList.Find(x => x.itemCode == itemCode);


        if (itemDetail != null)
        {
            texture = textureFromSprite(itemDetail.itemSprite);
            return itemDetail.itemDescription;
        }
        else
        {
            texture = null;
            return "No Item found.";
        }
    }


}
