using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Craft : MonoBehaviour
{
    //Dictionaries
    public Dictionary<string, int> tableMaterials;
    public Dictionary<string, int> inventory;

    //Lists
    public List<GameObject> onTableCards;
    public List<GameObject> inventoryItems;
    public List<CraftingRecipe> craftingRecipes;
    public List<Card> playerDeck;
    public List<GameObject> resultCards;

    //Database
    public CraftingRecipe[] recipeDatabase;
    
    //Game Objects
    public GameObject materialPrefab;
    public GameObject cardPrefab;
    public GameObject materialsArea;
    public GameObject resultArea;

    //Singleton
    public Singleton singleton;
    
    void Awake()
    {
        //Get Singleton
        singleton = GameObject.FindObjectOfType<Singleton>();
    }

    void Start()
    {
        //Load Database
        recipeDatabase = Resources.LoadAll<CraftingRecipe>("Craft Recipes");

        //Set Up Lists
        onTableCards = new List<GameObject>();
        resultCards = new List<GameObject>();
        inventoryItems = new List<GameObject>();
        craftingRecipes = new List<CraftingRecipe>();

        //Set Up Dictionaries
        tableMaterials = new Dictionary<string, int>();
        inventory = new Dictionary<string, int>();

        //Set Up Parent Areas
        resultArea = GameObject.Find("Result Area");
        materialsArea = GameObject.Find("Materials");
        
        //Grab Most Recent Player Deck
        playerDeck = GameObject.FindObjectOfType<Singleton>().playerDeck;
        
        //Add Recipe Detail Items from Database to List
        foreach(CraftingRecipe recipe in recipeDatabase)
        {
            craftingRecipes.Add(recipe);
        }

        //Create UI Inventory
        LoadInventory();
    }

    //Creates UI Inventory for Crafting
    void LoadInventory()
    {
        //Go through player deck
        foreach(Card card in playerDeck)
        {
            //if it already exists
            if(inventory.ContainsKey(card.cardName))
            {
                //increment by 1
                inventory[card.cardName] += 1;
            }
            else{
                InstantiateMaterialToInventory(card.cardName);
            }
        }
        UpdateInventoryAmount();
        GetAvailableCraftingOptions();
    }

    //Update count text on materials in inventory
    public void UpdateInventoryAmount(){
        
        for(int i = 0; i < inventoryItems.Count; i++)
        {
           CraftingMaterialBehavior materialScript = inventoryItems[i].GetComponent<CraftingMaterialBehavior>();
           materialScript.UpdateValue(inventory[materialScript.materialName]);
        }
    }

    //Update count text on cards on table
    public void UpdateOnTableAmount(){
        foreach(GameObject onTableCard in onTableCards)
        {
           
           CardBehavior cardScript = onTableCard.GetComponent<CardBehavior>();
           cardScript.UpdateQuantity(tableMaterials[cardScript.card.cardName]);
        }
    }

    //Add cards to the table
    public void AddToTable(Card card)
    {
        if(tableMaterials.ContainsKey(card.cardName))
        {
            tableMaterials[card.cardName] += 1;
            UpdateOnTableAmount();
        }
        else
        {
            tableMaterials.Add(card.cardName, 1);
            UpdateOnTableAmount();
        }
        GetAvailableCraftingOptions();
    }

    //Remove card objects from Table and Dictionary
    public void RemoveFromTable(GameObject cardObject)
    {
        //Need to fix issue of result cards not being removed when requirements don't meet anymore after removing material
        //Possibly a secondary clause in the places with get craftable quantity. If it comes back as 0, destroy it. 
        
        CardBehavior cardBehavior = cardObject.GetComponent<CardBehavior>();

        string cardName = cardBehavior.card.cardName;
        tableMaterials[cardName] -= 1;

        UpdateOnTableAmount();
        AddToInventory(cardName);

        if(tableMaterials[cardName] <= 0)
        {
            for(int i = 0; i < onTableCards.Count; i++)
            {
                if(onTableCards[i].GetComponent<CardBehavior>().card.cardName == cardName)
                {
                    onTableCards.Remove(onTableCards[i]);
                    Destroy(cardObject);   
                    break;
                }
            }
            
        }
        GetAvailableCraftingOptions();
    }

    //Add item qtys to inventory dictionary/instantiate if needed
    public void AddToInventory(string material){ 
        if(inventory.ContainsKey(material))
        {
            inventory[material] += 1;
        }
        else
        {
            InstantiateMaterialToInventory(material);
        }
        UpdateInventoryAmount();
    }

    //Create material item UI objects
    public void InstantiateMaterialToInventory(string material){
        //add listing to dictionary
        inventory.Add(material, 1);

        //instantiate material object into inventory
        GameObject materialObject = GameObject.Instantiate(materialPrefab, new Vector2(0,0), Quaternion.identity) as GameObject;
        CraftingMaterialBehavior materialBehavior = materialObject.GetComponent<CraftingMaterialBehavior>();

        materialObject.transform.SetParent(materialsArea.transform);
        inventoryItems.Add(materialObject);
        materialBehavior.materialName = material;

        //find matching card from player deck
        foreach(Card card in playerDeck){
            if(card.cardName == material){
                materialObject.GetComponent<Image>().sprite = card.cardImage;
                materialBehavior.card = card;
                break;
            }
        }            
    }

    //What can be made with cards on the table?
    public List<Card> GetAvailableCraftingOptions()
    {
        List<Card> availableCraftingOptions = new List<Card>();
        foreach(CraftingRecipe recipe in craftingRecipes) 
        {
            if(CanCraft(recipe))
            {
                //if there is a card already instantiated
                bool isInstantiated = false;
                foreach(GameObject cardObject in resultCards){ 
                    CardBehavior cardScript = cardObject.GetComponent<CardBehavior>();
                    if(recipe.resultItem.cardName == cardScript.card.cardName){
                        isInstantiated = true;

                        cardScript.card.quantity = GetCraftableQty(recipe);
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
            else
            {
                if(resultCards != null && resultCards.Count != 0){
                    for(int i = 0; i < resultCards.Count; i++)
                    {
                        if(resultCards[i].GetComponent<CardBehavior>().card.cardName == recipe.resultItem.cardName)
                        {
                            Destroy(resultCards[i].gameObject);
                            resultCards.Remove(resultCards[i]);
                        }
                    }
                }
            }
           
        }
        return availableCraftingOptions;
    }

    public bool CanCraft(CraftingRecipe recipe)
    {
        bool canCraft = true;
            foreach(CraftingMaterial material in recipe.craftingMaterials)
            {
                    if (!tableMaterials.ContainsKey(material.key) || tableMaterials[material.key] < material.amount) {
                        canCraft = false;
                        break;
                    } 
            }
            return canCraft;
    }

    //How many of each item can I make?
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

    //Let's make this thing
    public void CraftItem(){
        for(int i = 0; i < resultCards.Count; i++){
            Card card = resultCards[0].GetComponent<CardBehavior>().card;
            int quantityToMake = card.quantity;
            for(int j = 0; j < quantityToMake; j++)
            {
                singleton.playerDeck.Add(card);
                UseMaterials();
                card.quantity--;
            }   
            Destroy(resultCards[0].gameObject);
            card.quantity = 0;
            resultCards.Remove(resultCards[0]);
        } 
    }

    public void UseMaterials(){

    }


}
