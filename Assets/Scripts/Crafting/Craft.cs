using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Craft : MonoBehaviour
{
    // Dictionaries
    public Dictionary<string, int> tableMaterials = new Dictionary<string, int>();
    public Dictionary<string, int> inventory = new Dictionary<string, int>();

    // Lists
    public List<GameObject> onTableCards = new List<GameObject>();
    public List<GameObject> inventoryItems = new List<GameObject>();
    public List<CraftingRecipe> craftingRecipes = new List<CraftingRecipe>();
    public List<Card> playerDeck = new List<Card>();
    public List<GameObject> resultCards = new List<GameObject>();

    // Database
    public CraftingRecipe[] recipeDatabase;

    // Game Objects
    public GameObject materialPrefab;
    public GameObject cardPrefab;
    public GameObject materialsArea;
    public GameObject resultArea;

    // Singleton
    private Singleton singleton;

    private void Awake()
    {
        // Get Singleton
        singleton = GameObject.FindObjectOfType<Singleton>();
    }

    private void Start()
    {
        // Load Database
        recipeDatabase = Resources.LoadAll<CraftingRecipe>("Craft Recipes");

        // Set Up Lists
        craftingRecipes.AddRange(recipeDatabase);
        
        // Set Up Parent Areas
        resultArea = GameObject.Find("Result Area");
        materialsArea = GameObject.Find("Materials");

        // Grab Most Recent Player Deck
        playerDeck = singleton.playerDeck;

        // Create UI Inventory
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
                //Create visual material item
                InstantiateMaterialToInventory(card.cardName);
            }
        }
        UpdateInventoryAmount();
        GetAvailableCraftingOptions();
    }

   // Update count text on materials in inventory
    public void UpdateInventoryAmount()
    {
        foreach (GameObject item in inventoryItems)
        {
            CraftingMaterialBehavior materialScript = item.GetComponent<CraftingMaterialBehavior>();
            materialScript.UpdateValue(inventory[materialScript.materialName]);
        }
    }

    // Update count text on cards on table
    public void UpdateOnTableAmount()
    {
        foreach (GameObject onTableCard in onTableCards)
        {
            CardBehavior cardScript = onTableCard.GetComponent<CardBehavior>();
            cardScript.UpdateQuantity(tableMaterials[cardScript.card.cardName]);
        }
    }

   // Add card object to the table and Dictionary
    public void AddToTable(Card card)
    {
        if (tableMaterials.ContainsKey(card.cardName))
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

    //Remove card object from Table and Dictionary
    public void RemoveFromTable(GameObject cardObject)
    {
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

//Do you have enough materials on the table to qualify for a recipe
public bool CanCraft(CraftingRecipe recipe)
{
    return recipe.craftingMaterials.All(material => 
        tableMaterials.ContainsKey(material.key) && tableMaterials[material.key] >= material.amount);
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
    public void CraftItem() {
        GameObject firstResultCard = resultCards[0];
        Card card = firstResultCard.GetComponent<CardBehavior>().card;
        int quantityToMake = card.quantity;
        
        List<Card> cardsToAdd = new List<Card>();
        for (int j = 0; j < quantityToMake; j++) {
            cardsToAdd.Add(card);
            UseMaterials();
            card.quantity--;
        }
        singleton.playerDeck.AddRange(cardsToAdd);

        Destroy(firstResultCard.gameObject);
        card.quantity = 0;
        resultCards.Remove(firstResultCard);
    }

    //Remove the Cards in the player deck that were used to craft
    public void UseMaterials() {
        CraftingRecipe recipeMatch = FindRecipeMatch();

        if (recipeMatch != null) {
            foreach (CraftingMaterial material in recipeMatch.craftingMaterials) {
                UpdateTableMaterial(material);
            }

            RemoveMatchingCardsFromDeck(recipeMatch.craftingMaterials);
        }
    }

    //Find the matching recipe to the crafted card(s)
    private CraftingRecipe FindRecipeMatch() {
        foreach (GameObject resultCard in resultCards) {
            CardBehavior cardBehavior = resultCard.GetComponent<CardBehavior>();

            foreach (CraftingRecipe recipe in craftingRecipes) {
                if (recipe.resultItem.cardName == cardBehavior.card.cardName) {
                    return recipe;
                }
            }
        }

        return null;
    }

    //Either reduce qty or remove card objects from crafting table
    private void UpdateTableMaterial(CraftingMaterial material) {
        foreach (GameObject onTableCard in onTableCards) {
            CardBehavior cardBehavior = onTableCard.GetComponent<CardBehavior>();

            if (material.key == cardBehavior.card.cardName) {
                tableMaterials[material.key] -= material.amount;

                int newQty = tableMaterials[material.key];

                if (newQty == 0) {
                    Destroy(onTableCard);
                } else {
                    cardBehavior.UpdateQuantity(newQty);
                }

                break;
            }
        }
    }

    //Remove the used up cards in the player deck
    private void RemoveMatchingCardsFromDeck(CraftingMaterial[] materials) {
        foreach (CraftingMaterial material in materials) {
            for (int i = 0; i < material.amount; i++) {
                singleton.RemoveCardFromDeck(material.key);
            }
        }
    }

}
