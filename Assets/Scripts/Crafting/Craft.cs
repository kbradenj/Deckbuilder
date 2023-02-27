using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Craft : MonoBehaviour
{

    public Dictionary<string, int> tableMaterials;
    public Dictionary<string, int> inventory;

    List<GameObject> inventoryItems;
    List<CraftingRecipe> craftingRecipes = new List<CraftingRecipe>();


    public CraftingRecipe[] recipeDatabase;
    public List<Card> playerDeck;
    public List<GameObject> resultCards;

    public GameObject materialPrefab;
    public GameObject cardPrefab;
    public GameObject materialsArea;
    public GameObject resultArea;

    public Card cardToCraft;

    //Singleton
    public Singleton singleton;
    
    void Awake()
    {
        singleton = GameObject.FindObjectOfType<Singleton>();
    }

    void Start()
    {
        resultArea = GameObject.Find("Result Area");
        resultCards = new List<GameObject>();
        inventoryItems = new List<GameObject>();
        playerDeck = GameObject.FindObjectOfType<Singleton>().playerDeck;
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
                inventoryItems.Add(materialObject);

                CraftingMaterialBehavior materialBehavior = materialObject.GetComponent<CraftingMaterialBehavior>();
                materialBehavior.materialName = card.cardName;
                materialBehavior.card = card;
            }
        }
        UpdateInventoryAmount();
        GetAvailableCraftingOptions();
    }

    public void UpdateInventoryAmount(){
        
        foreach(GameObject material in inventoryItems)
        {
           CraftingMaterialBehavior materialScript = material.GetComponent<CraftingMaterialBehavior>();
           materialScript.UpdateValue(inventory[materialScript.materialName]);
        }

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
        foreach(CraftingRecipe recipe in craftingRecipes) 
        {
            bool canCraft = true;
            foreach(CraftingMaterial material in recipe.craftingMaterials)
            {
                    if (!tableMaterials.ContainsKey(material.key) || tableMaterials[material.key] < material.amount) {
                        canCraft = false;
                        break;
                    } 
            }
            if(canCraft)
            {
                //if there is a card already instantiated
                bool isInstantiated = false;
                foreach(GameObject cardObject in resultCards){ 
                    CardBehavior cardScript = cardObject.GetComponent<CardBehavior>();
                    if(recipe.resultItem.cardName == cardScript.card.cardName){
                        isInstantiated = true;

                        
                        cardScript.card.quantity = GetCraftableQty(recipe);;
                        cardScript.UpdateQuantity(cardScript.card.quantity);
                      
                        break;
                    }
                }
                //if there isn't a card already instantiated
                if(!isInstantiated){
                    availableCraftingOptions.Add(recipe.resultItem);
                    GameObject resultObject = GameObject.Instantiate(cardPrefab, new Vector2(0,0), Quaternion.identity) as GameObject;
                    resultCards.Add(resultObject);
                    resultObject.transform.SetParent(resultArea.transform);
                    CardBehavior resultItemScript = resultObject.GetComponent<CardBehavior>();
                    resultItemScript.displayAmount = true;
                    resultItemScript.RenderCard(recipe.resultItem);
                    resultItemScript.card.quantity = GetCraftableQty(recipe);
                    resultItemScript.UpdateQuantity(resultItemScript.card.quantity);
                }
            }
           
        }
        return availableCraftingOptions;
    }

    public int GetCraftableQty(CraftingRecipe recipe){
        int amountToCraft = int.MaxValue;
        int tempAmount;
        foreach(CraftingMaterial material in recipe.craftingMaterials){
            tempAmount = tableMaterials[material.key]/material.amount;
            if(tempAmount < amountToCraft)
            {
                amountToCraft = tempAmount;
            }

        }
        return amountToCraft;    
    }

    public void CraftItem(){
        for(int i = 0; i < resultCards.Count; i++){
            Card card = resultCards[0].GetComponent<CardBehavior>().card;
            int quantityToMake = card.quantity;
            for(int j = 0; j < quantityToMake; j++)
            {
                singleton.playerDeck.Add(card);
                card.quantity--;
            }   
            Destroy(resultCards[0].gameObject);
            card.quantity = 0;
            resultCards.Remove(resultCards[0]);
        } 
    }


}
