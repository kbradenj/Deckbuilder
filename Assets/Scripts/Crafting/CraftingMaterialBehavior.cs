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
    public CraftingRecipe recipe;
    public GameObject cardPrefab;
    public TMP_Text materialCountText;

    void Awake()
    {
        craft = GameObject.FindObjectOfType<Craft>();
        craftingTable = GameObject.Find("Crafting Area");
    }

    public void UpdateValue(int newAmount)
    {
        materialCountText.text = newAmount.ToString();
    }

    public void MoveToTable()
    {  
         if(!craft.isRecipeView){
            //Did we already instantiate this material's Card on the table?
            if(!craft.tableMaterials.TryGetValue(materialName, out amount) || craft.tableMaterials[materialName] == 0)
            {
               InstantiateToTable();
            }

            //Adds to the dictionary and updates QTY shown on card stack on table
            craft.AddToTable(card);
            RemoveFromInventory(materialName);
        }
        else
        {
            MakeFromRecipe();
            
        }
        
    }

    public void RemoveFromInventory(string material)
    {
        if(!craft.isRecipeView){
            int tempCount = craft.inventory[materialName];
            tempCount -= 1;
            craft.inventory[materialName] = tempCount;
            //Did we use them all?
            if(tempCount <= 0)
            {
                craft.inventory.Remove(material);
                for(int i = 0; i < craft.inventoryItems.Count; i++)
                {
                    if(craft.inventoryItems[i].GetComponent<CraftingMaterialBehavior>().materialName == material)
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
        else
        {
            //Did we use them all?
            if(amount <= 0)
            {
                craft.inventory.Remove(material);
                for(int i = 0; i < craft.inventoryItems.Count; i++)
                {
                    if(craft.inventoryItems[i].GetComponent<CraftingMaterialBehavior>().materialName == material)
                    {
                        craft.inventoryItems.Remove(craft.inventoryItems[i]);
                        break;
                    }
                }
                Destroy(this.gameObject);
            }
            else
            {
                UpdateValue(amount);
            }
        }
        
    }

    public void RemoveRecipeFromView()
    {

    }

    public void InstantiateToTable()
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

    public void MakeFromRecipe()
    {
        //Go through each material inside of this recipe
       foreach(CraftingMaterial material in recipe.craftingMaterials)
       {
        //Go through each card in the player deck to match it with a materials
         for(int i = 0; i < craft.playerDeck.Count; i++)
         {
            if(material.key == craft.playerDeck[i].cardName)
            {
                //found an instance of the card
                card = craft.playerDeck[i];
                break;
            }
         }
         for(int j = 0; j < material.amount; j++)
         {
            materialName = material.key;
            int testInt = int.MaxValue;
            if(!craft.tableMaterials.TryGetValue(materialName, out testInt) || craft.tableMaterials[materialName] == 0)
            {
               InstantiateToTable();
            }
            //Adds to the dictionary and updates QTY shown on card stack on table
            craft.AddToTable(card);
            craft.isTableLoaded = true;
            RemoveFromInventory(material.key);
         }
       }
        amount--;
        UpdateValue(amount);
        if(amount < 1)
        {
            for(int i = 0; i < craft.inventoryItems.Count; i++)
            {
                if(craft.inventoryItems[i].GetComponent<CraftingMaterialBehavior>().recipe.resultItem.cardName == recipe.resultItem.cardName)
                {
                    Destroy(craft.inventoryItems[i].gameObject);
                    craft.inventoryItems.Remove(craft.inventoryItems[i]);       
                }
            }
        }
    }
}
