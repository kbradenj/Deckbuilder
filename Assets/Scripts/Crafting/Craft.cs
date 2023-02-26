using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Craft : MonoBehaviour
{

    public Dictionary<string, int> tableMaterials;
    public Dictionary<string, int> inventory;
    List<CraftingRecipe> craftingRecipes = new List<CraftingRecipe>();

    public CraftingRecipe[] recipeDatabase;
    public List<Card> playerDeck;

    public GameObject materialPrefab;
    public GameObject cardPrefab;
    public GameObject materialsArea;
    
    void Start()
    {
        playerDeck = GameObject.FindObjectOfType<GameState>().playerDeck;
        tableMaterials = new Dictionary<string, int>();
        inventory = new Dictionary<string, int>();
        materialsArea = GameObject.Find("Materials");
        recipeDatabase = Resources.LoadAll<CraftingRecipe>("Craft Recipes");
        foreach(CraftingRecipe recipe in recipeDatabase)
        {
            craftingRecipes.Add(recipe);
        }
        LoadInventory();
    }

    void LoadInventory()
    {
        foreach(Card card in playerDeck)
        {
            if(inventory.ContainsKey(card.cardName))
            {
                inventory[card.cardName] += 1;
            }
            else{
                inventory.Add(card.cardName, 1);
                GameObject materialObject = GameObject.Instantiate(materialPrefab, new Vector2(0,0), Quaternion.identity) as GameObject;
                materialObject.transform.SetParent(materialsArea.transform);
                materialObject.GetComponent<Image>().sprite = card.cardImage;
                CraftingMaterialBehavior materialBehavior = materialObject.GetComponent<CraftingMaterialBehavior>();
                materialBehavior.materialName = card.cardName;
                materialBehavior.card = card;
            }
            
        }
        GetAvailableCraftingOptions();
    }

    public void AddToTable(string material)
    {
        if(tableMaterials.ContainsKey(material))
        {
            tableMaterials[material] += 1;
        }
        else
        {
            tableMaterials.Add(material, 1);
        }
        GetAvailableCraftingOptions();
    }

    public List<Card> GetAvailableCraftingOptions()
    {

        List<Card> availableCraftingOptions = new List<Card>();
        foreach(CraftingRecipe recipe in craftingRecipes) {
            bool canCraft = true;
            foreach(CraftingMaterial material in recipe.craftingMaterials)
            {
                    if (!tableMaterials.ContainsKey(material.key) || tableMaterials[material.key] < material.amount) {
                        canCraft = false;
                        break;
                    }
            }

            if(canCraft){
                
                availableCraftingOptions.Add(recipe.resultItem);
                GameObject resultObject = GameObject.Instantiate(cardPrefab, new Vector2(0,0), Quaternion.identity) as GameObject;
                resultObject.transform.SetParent(GameObject.Find("Result Area").transform);
                resultObject.GetComponent<CardBehavior>().RenderCard(recipe.resultItem);
            }
           
        }
        return availableCraftingOptions;
    }
}
