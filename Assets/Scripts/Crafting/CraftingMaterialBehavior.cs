using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingMaterialBehavior : MonoBehaviour
{
    public GameObject craftingTable;
    public int amount;
    public string materialName;
    public Image materialImage;
    public Craft craft;
    public Card card;
    public GameObject cardPrefab;
    public TMP_Text materialCountText;

    void Awake()
    {
        craft = GameObject.FindObjectOfType<Craft>();
        craftingTable = GameObject.Find("Crafting Area");
        amount = 3;
    }

    public void UpdateValue(int newAmount)
    {
        materialCountText.text = newAmount.ToString();
    }

    public void MoveToTable()
    {
            //Did we already instantiate this material's Card on the table?
            if(!craft.tableMaterials.TryGetValue(materialName, out amount) || craft.tableMaterials[materialName] == 0)
            {
                //Instantiate Card Object
                GameObject usedMaterial = GameObject.Instantiate(cardPrefab, new Vector2 (0,0), Quaternion.identity);

                //Add remove from table script
                usedMaterial.AddComponent<RemoveFromTable>();

                //Add table object to list
                craft.onTableCards.Add(usedMaterial);

                //Set Up UI
                CardBehavior cardBehavior = usedMaterial.GetComponent<CardBehavior>();
                cardBehavior.RenderCard(card, true);
                usedMaterial.transform.localScale *= 0.5f;
                usedMaterial.transform.SetParent(craftingTable.transform);
            }

            //Adds to the dictionary and updates QTY shown on card stack on table
            craft.AddToTable(card);

            //Remove amount in inventory (not actual player deck)
            int tempCount = craft.inventory[materialName];
            tempCount -= 1;
            craft.inventory[materialName] = tempCount;

            //Did we use them all?
            if(tempCount <= 0)
            {
                craft.inventory.Remove(materialName);
                for(int i = 0; i < craft.inventoryItems.Count; i++)
                {
                    if(craft.inventoryItems[i].GetComponent<CraftingMaterialBehavior>().materialName == materialName)
                    {
                        craft.inventoryItems.Remove(craft.inventoryItems[i]);
                        break;
                    }
                }
                Destroy(this.gameObject);
            }
            else
            {
                UpdateValue(tempCount);
            }
           
    }
}
