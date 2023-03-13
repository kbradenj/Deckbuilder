using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class Craft : MonoBehaviour
{
    // Dictionaries
    public Dictionary<string, int> tableMaterials = new Dictionary<string, int>();
    public Dictionary<string, int> inventory = new Dictionary<string, int>();
    public Dictionary<string, int> craftableRecipes = new Dictionary<string, int>();

    // Lists
    public List<GameObject> onTableCards = new List<GameObject>();
    public List<GameObject> inventoryItems = new List<GameObject>();
    public List<CraftingRecipe> craftingRecipes = new List<CraftingRecipe>();
    public List<Card> playerDeck = new List<Card>();
    public GameObject resultCard;

    // Database
    public CraftingRecipe[] recipeDatabase;

    // Game Objects
    public GameObject materialPrefab;
    public GameObject cardPrefab;
    public GameObject materialsArea;
    public GameObject resultArea;
    public GameObject craftCostPrefab;

    // Singleton
    private Singleton singleton;

    //TMPro
    TMP_Text craftCostText;

    //Color
    public Color disabledColor;

    //States
    public bool isRecipeView = false;
    public bool isInventoryLoaded = false;
    public bool isTableLoaded = false;

    private void Awake()
    {
        // Get Singleton
        singleton = GameObject.FindObjectOfType<Singleton>();
        singleton.AdjustDaylight();
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
    public void LoadInventory()
    {
        if(!isInventoryLoaded)
        {
            if(isRecipeView == true){
                isRecipeView = false;
                if(resultCard != null)
                {
                    Destroy(resultCard);
                    resultCard = null;
                }
             
                int initialInvCount = inventoryItems.Count;
                for(int i = 0; i < initialInvCount; i++)
                {
                    Destroy(inventoryItems[i].gameObject);
                }
                inventoryItems.Clear();
                inventory.Clear();
                ClearTable();
                craftableRecipes.Clear();
            }
        
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
            // GetAvailableCraftingOptions();
            isInventoryLoaded = true;
        }
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

   // Add card Dictionary
    public void AddToTable(Card card)
    {
        if (tableMaterials.ContainsKey(card.cardName))
        {
            tableMaterials[card.cardName] += 1;
            card.quantity += 1;
            UpdateOnTableAmount();
        }
        else
        {
            tableMaterials.Add(card.cardName, 1);
            card.quantity += 1;
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
        cardBehavior.card.quantity -=1;

        UpdateOnTableAmount();
        AddToInventory(cardName);

        if(tableMaterials[cardName] <= 0)
        {
            for(int i = 0; i < onTableCards.Count; i++)
            {
                if(onTableCards[i].GetComponent<CardBehavior>().card.cardName == cardName)
                {
                    onTableCards.Remove(onTableCards[i]);
                    tableMaterials.Remove(cardName);
                    Destroy(cardObject);                  
                    break;
                }
            }
        }
        if(GetAvailableCraftingOptions().Count > 0)
        {
            GetAvailableCraftingOptions();
        }
        else
        {
            if(resultCard != null)
            {
                Destroy(resultCard.gameObject);
                resultCard = null;
            }
            
        }
    }

    public void ClearTable()
    {
        int initialTableCount = onTableCards.Count;
        for(int i = 0; i < initialTableCount; i++)
        {
            CardBehavior cardBehavior = onTableCards[i].GetComponent<CardBehavior>();
            cardBehavior.UpdateQuantity(0);
            cardBehavior.card.quantity = 0;
            Destroy(onTableCards[i].gameObject);
        }
        onTableCards.Clear();
        tableMaterials.Clear();
    }

    //Add item qtys to inventory dictionary/instantiate if needed
    public void AddToInventory(string material){ 

        if(inventory.ContainsKey(material))
        {
            inventory[material] += 1;
        }
        else
        {
            if(!isRecipeView)
            {
                InstantiateMaterialToInventory(material);
            }
        }
        UpdateInventoryAmount();
    }

    //Create material item UI objects
    public void InstantiateMaterialToInventory(string material){

        //add listing to dictionary
        if(!isRecipeView)
        {
             inventory.Add(material, 1);
        }
        else
        {
            craftableRecipes.Add(material, 1);
        }
    
        //instantiate material object into inventory
        CreateMaterialObject(material);     
    }

    public void CreateMaterialObject(string material)
    {

        GameObject materialObject = GameObject.Instantiate(materialPrefab, new Vector2(0,0), Quaternion.identity) as GameObject;
        CraftingMaterialBehavior materialBehavior = materialObject.GetComponent<CraftingMaterialBehavior>();

        materialObject.transform.SetParent(materialsArea.transform);
        inventoryItems.Add(materialObject);
        materialBehavior.materialName = material;

        foreach(Card card in singleton.cardDatabase){
            if(card.cardName == material){
                materialObject.GetComponent<Image>().sprite = card.cardImage;
                materialBehavior.card = card;
                break;
            }
        }    

        if(isRecipeView)
        {
            //find matching recipe to card that was clicked
            foreach(CraftingRecipe recipe in recipeDatabase){
                if (recipe.resultItem.cardName == materialBehavior.card.cardName)
                {
                    materialBehavior.recipe = recipe;   
                    materialBehavior.amount = GetCraftableQty(materialBehavior.recipe);
                    materialBehavior.UpdateValue(materialBehavior.amount);
                }
            }
        }  
    }

    public void PossibleRecipeView()
    {
      
        if(!isRecipeView)
        {
            ClearTable();
            if(resultCard != null)
            {
                Destroy(resultCard.gameObject);
                resultCard = null;
            }
            isRecipeView = true;
            int initialInvCount = inventoryItems.Count;
            for(int i = 0; i < initialInvCount; i++)
            {
                Destroy(inventoryItems[i].gameObject);
            }
            //Remove game objects from inventoryItems list
            inventoryItems.Clear(); //inventoryItemsSize = 0

            //Go through recipes
            foreach(CraftingRecipe recipe in craftingRecipes)
            {
                //if it doesn't exist
                if(!craftableRecipes.ContainsKey(recipe.resultItem.cardName) && CanCraft(recipe))
                {
                    //Add Recipe to CraftableRecipes
                    InstantiateMaterialToInventory(recipe.resultItem.cardName);
                 
                }
            }
        }
        isInventoryLoaded = false;
    }


    //What can be made with cards on the table?
    public List<Card> GetAvailableCraftingOptions()
    {
        List<Card> availableCraftingOptions = new List<Card>();
        foreach(CraftingRecipe recipe in craftingRecipes) 
        {
            if(CanCraft(recipe) && !HasExtraMaterial(recipe))
            {
                //if there is a card already instantiated
                if(resultCard != null)
                {
                    CardBehavior cardScript = resultCard.GetComponent<CardBehavior>();
                    if(recipe.resultItem.cardName == cardScript.card.cardName){
                        cardScript.card.quantity = GetCraftableQty(recipe);
                        cardScript.UpdatePriceText("Crafting Time: " + (recipe.resultItem.quantity * recipe.timeCost).ToString() + " min");
                        cardScript.UpdateQuantity(cardScript.card.quantity);
                    }
                }
                else
                {
                    availableCraftingOptions.Add(recipe.resultItem);
                    RenderResultCard(recipe);
                    return availableCraftingOptions;
                }
            }
        }
        return availableCraftingOptions;
    }

    public void RenderResultCard(CraftingRecipe recipe)
    {
           GameObject resultObject = GameObject.Instantiate(cardPrefab, new Vector2(0,0), Quaternion.identity) as GameObject;
            resultCard = resultObject;
            resultObject.transform.SetParent(resultArea.transform);
            CardBehavior resultItemScript = resultObject.GetComponent<CardBehavior>();
            resultItemScript.RenderCard(recipe.resultItem, true);
            resultItemScript.card.quantity = GetCraftableQty(recipe);
            resultItemScript.UpdateQuantity(resultItemScript.card.quantity);
            resultItemScript.AddPrice(resultObject, recipe.timeCost);
    }

    public bool HasExtraMaterial(CraftingRecipe recipe)
    {
        foreach(KeyValuePair<string, int> kvp in tableMaterials)
        {
            bool matched = false;
            foreach(CraftingMaterial material in recipe.craftingMaterials)
            {
                if(matched == true)
                {
                    break;
                }
                else
                {
                    if(kvp.Key == material.key){
                    matched = true;
                    }
                    else
                    {
                        matched = false;
                    }
                } 
            }
            if(matched == false){
                return true;
            }
        }
        return false;
    }

    public void AddCraftingCost(GameObject card, int craftCost)
    {
        GameObject craftCostObject = GameObject.Instantiate(craftCostPrefab, new Vector2(0,250), Quaternion.identity);
        craftCostObject.transform.SetParent(card.transform);
        craftCostText = GameObject.Find("Craft Cost Text").GetComponent<TMP_Text>();
        craftCostText.text = "Crafting Time: " + craftCost.ToString() + " min";
    }

    //Do you have enough materials on the table to qualify for a recipe
    public bool CanCraft(CraftingRecipe recipe)
    {
        if(!isRecipeView)
        {
        return recipe.craftingMaterials.All(material => 
            tableMaterials.ContainsKey(material.key) && tableMaterials[material.key] >= material.amount && HasExtraMaterial(recipe) == false);
        }
        else
        {
            return recipe.craftingMaterials.All(material => 
            inventory.ContainsKey(material.key) && inventory[material.key] >= material.amount);
        }
    }

    //How many of each item can I make?
    public int GetCraftableQty(CraftingRecipe recipe){
        int amountToCraft = int.MaxValue;
        int tempAmount;
        foreach(CraftingMaterial material in recipe.craftingMaterials){

            if(!isRecipeView || isTableLoaded)
            {
                tempAmount = tableMaterials[material.key]/material.amount;
            }
            else
            {
                tempAmount = inventory[material.key]/material.amount;
            }
           
            if(tempAmount < amountToCraft)
            {
                amountToCraft = tempAmount;
            }
        }
        return amountToCraft;    
    }

    //Let's make this thing
    public void CraftItem() {
        Card card = resultCard.GetComponent<CardBehavior>().card;
        int quantityToMake = card.quantity;
        
        List<Card> cardsToAdd = new List<Card>();
        for (int j = 0; j < quantityToMake; j++) {
            cardsToAdd.Add(card);
            UseMaterials();
            card.quantity--;
        }
        singleton.playerDeck.AddRange(cardsToAdd);
        for(int i = 0; i < quantityToMake; i++)
        {
            AddToInventory(card.cardName);
        }
        singleton.AdjustDaylight(0);
        Destroy(resultCard.gameObject);
        card.quantity = 0;
        cardsToAdd.Clear();
        resultCard = null;
    }

    //Remove the Cards in the player deck that were used to craft
    public void UseMaterials() {
        CraftingRecipe recipeMatch = FindRecipeMatch();

        if (recipeMatch != null) {
            singleton.AdjustDaylight(recipeMatch.timeCost);
            foreach (CraftingMaterial material in recipeMatch.craftingMaterials) {
                UpdateTableMaterial(material);
            }

            RemoveMatchingCardsFromDeck(recipeMatch.craftingMaterials);
        }
    }

    //Find the matching recipe to the crafted card(s)
    private CraftingRecipe FindRecipeMatch() {

            CardBehavior cardBehavior = resultCard.GetComponent<CardBehavior>();
            foreach (CraftingRecipe recipe in craftingRecipes) {
                if (recipe.resultItem.cardName == cardBehavior.card.cardName) {
                    return recipe;
                }
            }

        return null;
    }

    //Either reduce qty or remove card objects from crafting table
    private void UpdateTableMaterial(CraftingMaterial material) {
        for (int i = 0; i < onTableCards.Count; i++) {
            CardBehavior cardBehavior = onTableCards[i].GetComponent<CardBehavior>();

            if (material.key == cardBehavior.card.cardName) {
                tableMaterials[material.key] -= material.amount;
                int newQty = tableMaterials[material.key];

                if (newQty == 0) {
                    cardBehavior.card.quantity = 0;
                    cardBehavior.UpdateQuantity(0);
                    Destroy(onTableCards[i].gameObject);
                    onTableCards.Remove(onTableCards[i]);
                    tableMaterials.Remove(material.key);
                    
                } else {
                    cardBehavior.card.quantity = newQty;
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
