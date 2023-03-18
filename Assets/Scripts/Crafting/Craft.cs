using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Craft : MonoBehaviour
{
    //Dictionary
    public Dictionary<string, Card> playerDeck = new Dictionary<string, Card>();
    public Dictionary<string, CraftCard> inventoryCards = new Dictionary<string, CraftCard>();
    public Dictionary<string, CraftCard> tableCards = new Dictionary<string, CraftCard>();

    //Database
    public CraftingRecipe[] recipeDatabase;

    //Singular Card
    public GameObject resultCard = null;

    // Singleton
    private Singleton singleton;

    //GameObjects
    public GameObject compactPrefab;
    public GameObject cardVisualPrefab;

    //States
    public bool isRecipeView = false;
    public bool exists;

    // Areas
    public GameObject resultArea;
    public GameObject tableArea;
    public GameObject selectionArea;

    //Color
    public Color disabledColor;

    private void Awake()
    {
        // Get Singleton
        singleton = GameObject.FindObjectOfType<Singleton>();
        singleton.AdjustDaylight();

        // Set Up Parent Areas
        resultArea = GameObject.Find("Result Area");
        tableArea = GameObject.Find("Table Area");
        selectionArea = GameObject.Find("Selection Area");
    }

    private void Start()
    {
        // Load Database
        recipeDatabase = Resources.LoadAll<CraftingRecipe>("Craft Recipes");

        //Create Inventory Dictionary
        LoadPlayerDeckDictionary();
        LoadInventoryCardObjects();
    }

    public void InventoryView()
    {
        isRecipeView = false;
        ClearAll();
        LoadPlayerDeckDictionary();
        LoadInventoryCardObjects();
    }

    public void LoadPlayerDeckDictionary(){
        playerDeck.Clear();
        foreach(Card card in singleton.playerDeck)
        {
            if(playerDeck.ContainsKey(card.cardName)){
                
                playerDeck[card.cardName].deckQty++;
                continue;
            }
            else
            {
                playerDeck.Add(card.cardName, card);
                playerDeck[card.cardName].deckQty = 1;
            }
        }

    }

    public void LoadInventoryCardObjects()
    {
        foreach(KeyValuePair<string, Card> kvp in playerDeck)
        {
            for(int i = 0; i < kvp.Value.deckQty; i++)
            {
                 AddToInventory(kvp.Value);
            }
                
        }
    }

    public void RemoveCard(Dictionary<string, CraftCard> area, CraftCard cardScript)
    {
            string cardName = cardScript.card.cardName;
            if(area.ContainsKey(cardName))
            {
                if(cardScript.qty > 1)
                {
                    cardScript.qty--;
                    cardScript.Render();
                }
                else
                {
                    cardScript.qty--;
                    cardScript.Render();
                    Destroy(cardScript.gameObject);
                    area.Remove(cardName);
                }
            }
        GetAvailableCraftingOptions();
    }

//possibly need to break out the instantiation part of this into its own script
    public void AddToInventory(Card card){
        string cardName = card.cardName;
        CraftCard newCraftCard;
        if(inventoryCards.ContainsKey(cardName))
        {
            inventoryCards[cardName].qty++;
            inventoryCards[cardName].Render();
        }
        else
        {
            GameObject invCard = GameObject.Instantiate(compactPrefab, Vector2.zero, Quaternion.identity);
            invCard.transform.SetParent(selectionArea.transform);
            if(!isRecipeView){
                newCraftCard = invCard.AddComponent<SelectionCard>();
                newCraftCard.qty = 1;
            }
            else
            { 
                newCraftCard = invCard.AddComponent<RecipeCard>();
                newCraftCard.qty = GetCraftableQty(card.recipe);
                if(newCraftCard.qty == 0)
                {
                    Destroy(newCraftCard.gameObject);
                    return;
                }
            }

            newCraftCard.card = card;
            newCraftCard.Render();
            newCraftCard.isInstantiated = true;
            inventoryCards.Add(cardName, newCraftCard);
            if(isRecipeView)
            {
                if(!CanMakeRecipe(card.recipe) && !newCraftCard.isDisabled)
                {
                    newCraftCard.ToggleDisable();
                }
                if(CanMakeRecipe(card.recipe) && newCraftCard.isDisabled)
                {
                    newCraftCard.ToggleDisable();
                }
            }
            
        }
    }

    public void AddToTable(Card card){

        string cardName = card.cardName;
        if(tableCards.ContainsKey(cardName))
        {
            tableCards[cardName].qty++;
            tableCards[cardName].Render();
            GetAvailableCraftingOptions();
            return;
        }
        GameObject tableCard = GameObject.Instantiate(cardVisualPrefab, new Vector2(0,0), Quaternion.identity) as GameObject;
        tableCard.transform.SetParent(tableArea.transform);
        TableCard tableCardScript = tableCard.AddComponent<TableCard>();
        tableCardScript.qty = 1;
        tableCardScript.card = card;
        tableCardScript.Render();
        tableCard.transform.localScale *= 0.5f;
        tableCardScript.isInstantiated = true;
        tableCards.Add(cardName, tableCardScript);

        GetAvailableCraftingOptions();
    }

    public void ClearAll()
    {
        //Clear Inventory Cards
        List<string> inventoryCardKeys = new List<string>(inventoryCards.Keys);
        for(int i = 0; i < inventoryCardKeys.Count; i++)
        {
            string cardKey = inventoryCardKeys[i];
            inventoryCards[cardKey].qty = 0;
            Destroy(inventoryCards[cardKey].gameObject);
            inventoryCards.Remove(cardKey);
        }

        //Clear Table Cards
        List<string> tableCardKeys = new List<string>(tableCards.Keys);
        for(int i = 0; i < tableCardKeys.Count; i++)
        {
            string cardKey = tableCardKeys[i];
            tableCards[cardKey].qty = 0;
            Destroy(tableCards[cardKey].gameObject);
            tableCards.Remove(cardKey);
        }

        if(resultCard != null)
        {
            Destroy(resultCard.gameObject);
            resultCard = null;
        }
    }

        public void RenderResultCard(CraftingRecipe recipe)
    {
        GameObject resultCardInstance = GameObject.Instantiate(cardVisualPrefab, new Vector2(0,0), Quaternion.identity) as GameObject;
        resultCardInstance.transform.SetParent(resultArea.transform);
        CardBehavior resultItemScript = resultCardInstance.GetComponent<CardBehavior>();
        resultItemScript.RenderCard(recipe.resultItem, true);
        resultItemScript.card.quantity = GetCraftableQty(recipe);
        resultItemScript.UpdateQuantity(resultItemScript.card.quantity);
        resultItemScript.AddPrice(resultCardInstance, recipe.timeCost);
        resultItemScript.priceText = GameObject.Find("Craft Cost Text").GetComponent<TMP_Text>();
        resultItemScript.priceText.text = "Crafting Time: " + recipe.timeCost.ToString() + " min";
        resultCard = null;
        resultCard = resultCardInstance;
    }

    public void GetAvailableCraftingOptions()
    {
        foreach(CraftingRecipe recipe in recipeDatabase) 
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
                        return;
                    }
                }
                else
                {
                    RenderResultCard(recipe);
                    return;
                }
            }
            else
            {
                if(resultCard != null){
                    Destroy(resultCard.gameObject);
                    resultCard = null;
                }
                
            }
        }
    }

    public bool CanCraft(CraftingRecipe recipe)
    {
        bool canCraft = false;
        foreach(CraftingMaterial material in recipe.craftingMaterials){
            if(tableCards.ContainsKey(material.key))
            {  
                    if(tableCards[material.key].qty < material.amount)
                    {
                        return false;
                    }
                    else
                    {
                        canCraft = true;
                    }
            }
            else{
                return false;
            }
        } 
        if(canCraft == false){
            return false;
        }
        return canCraft;
    }

    public bool HasExtraMaterial(CraftingRecipe recipe){
        bool matched = false;
        Dictionary<string, CraftCard> dictionary = tableCards;
        foreach(KeyValuePair<string, CraftCard> kvp in dictionary){
            matched = false;
            foreach(CraftingMaterial material in recipe.craftingMaterials){
                if(matched == true){
                    if(kvp.Key == material.key){
                    matched = true;
                    }
                }
                else{
                    if(kvp.Key == material.key){
                        matched = true;
                        break;
                    }
                } 
            }
            if(matched == false){
                return true;
            }
        }
        return false;
    }

    public int GetCraftableQty(CraftingRecipe recipe){
    int amountToCraft = int.MaxValue;
    int tempAmount = 0;
    foreach(CraftingMaterial material in recipe.craftingMaterials){
            if(isRecipeView){
                tempAmount = playerDeck[material.key].deckQty/material.amount;
            }
            else
            {
                tempAmount = tableCards[material.key].qty/material.amount;
            }
            if(tempAmount < amountToCraft){
                amountToCraft = tempAmount;
            }
        }
    if(resultCard != null){
        CardBehavior resultCardBehavior = resultCard.GetComponent<CardBehavior>();
        resultCardBehavior.qty = amountToCraft;
        resultCardBehavior.RenderCard(resultCardBehavior.card, true);
    }
    return amountToCraft;    
    }

    //Let's make this thing
    public void CraftItem() {
        Card card = resultCard.GetComponent<CardBehavior>().card;
        int quantityToMake = card.quantity;
        for(int i = 0; i < quantityToMake; i++)
        {
            if(!isRecipeView)
            {
                AddToInventory(card);
            }
            
            UseMaterials();
            singleton.playerDeck.Add(card);
        }

        singleton.AdjustDaylight(0);
        card.quantity = 0;
        Destroy(resultCard.gameObject);
        resultCard = null;
    }

    //Remove the Cards in the player deck that were used to craft
    public void UseMaterials() {
        CraftingRecipe recipeMatch = FindRecipeMatch();
        if (recipeMatch != null) {
            singleton.AdjustDaylight(recipeMatch.timeCost);
            foreach (CraftingMaterial material in recipeMatch.craftingMaterials) {
                for(int i = 0; i < material.amount; i++)
                {
                    RemoveMatchingCardsFromDeck(material);
                }
                UpdateTableMaterial(material); 
            }
    
        }
    }

    //Find the matching recipe to the crafted card(s)
    private CraftingRecipe FindRecipeMatch() {
            CardBehavior cardBehavior = resultCard.GetComponent<CardBehavior>();
            foreach (CraftingRecipe recipe in recipeDatabase) {
                if (recipe.resultItem.cardName == cardBehavior.card.cardName) {
                    return recipe;
                }
            }
        return null;
    }

    //Either reduce qty or remove card objects from crafting table
    private void UpdateTableMaterial(CraftingMaterial material) {
            CraftCard craftCard = tableCards[material.key];
            craftCard.qty -= material.amount;
            int newQty = craftCard.qty;

            if (newQty == 0) {
                craftCard.qty = 0;
                craftCard.Render();
                Destroy(craftCard.gameObject);
                tableCards.Remove(material.key);
                return;
            } 
            else 
            {
                craftCard.qty = newQty;
                craftCard.Render();
            }
     }
    
    //Remove the used up cards in the player deck
    private void RemoveMatchingCardsFromDeck(CraftingMaterial material) {
            singleton.RemoveCardFromDeck(material.key);
    }
    
    //For Recipe View Only
    public bool CanMakeRecipe(CraftingRecipe recipe)
    {
        foreach(CraftingMaterial material in recipe.craftingMaterials)
        {
            if(!playerDeck.ContainsKey(material.key))
            {
                return false;
            }
        }
        return true;
    }

    //What happens when the player swaps over to the recipe view
    public void PossibleRecipeView()
    {
        //need to never delete inventory unless playerdeckloader is called;
            ClearAll();
            LoadPlayerDeckDictionary();
            isRecipeView = true;

            //Go through recipes
            foreach(CraftingRecipe recipe in recipeDatabase)
            {
                if(CanMakeRecipe(recipe)){
                      AddToInventory(recipe.resultItem);
                }
            }
    }

    public void MakeFromRecipe(CraftCard craftCard)
    {
    CraftingRecipe recipe = craftCard.card.recipe;
    if(CanMakeRecipe(recipe)){
        foreach(CraftingMaterial material in recipe.craftingMaterials)
        {
            //Go through each card in the player deck to match it with a materials
            for(int i = 0; i < material.amount; i++){
                AddToTable(playerDeck[material.key]);
            }
        }
    }
    }
}










