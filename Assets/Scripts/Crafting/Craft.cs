using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Craft : MonoBehaviour
{
    //Dictionary
    public Dictionary<string, int> inventory = new Dictionary<string, int>();
    
    //Lists
    public List<GameObject> inventoryCards = new List<GameObject>();
    public List<GameObject> recipeCards = new List<GameObject>();
    public List<GameObject> tableCards = new List<GameObject>();

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
    public bool isRecipeView;
    public bool exists;

    //TMPro

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
        LoadInventoryDictionary();
    }

    public void InventoryView()
    {
        ClearAll();
        LoadInventoryDictionary();
        isRecipeView = false;
    }

    public void ResetInventory(){
        Dictionary<string, int> tempDictionary = new Dictionary<string, int>();
        foreach(KeyValuePair<string, int> kvp in inventory){
            tempDictionary.Add(kvp.Key, 0);
        }
        inventory = tempDictionary;
        inventory.Clear();
    }
    public void LoadInventoryDictionary(){
        ResetInventory();
        foreach(Card card in singleton.playerDeck)
        {
            if(inventory.TryGetValue(card.cardName, out int count)){
                
                inventory[card.cardName]++;
            }
            else
            {
                inventory.Add(card.cardName, 1);
            }
        }
        LoadInventoryCards();
    }

    public void LoadInventoryCards()
    {
        Card cardToLoad = null;
        foreach(KeyValuePair<string, int> kvp in inventory)
        {
            foreach(Card dbCard in singleton.cardDatabase){
                cardToLoad = dbCard;
                if(dbCard.cardName == kvp.Key)
                {
                    for(int i = 0; i < kvp.Value; i++)
                    {
                        AddToInventory(cardToLoad);
                    }
                } 
            }
        }
    }

    public void RemoveFromList(List<GameObject> list, CraftCard cardScript)
    {
        for(int i = 0; i < list.Count; i++)
        {
            if(list[i].GetComponent<CraftCard>().card.cardName == cardScript.card.cardName)
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
                    Destroy(list[i].gameObject);
                    list.Remove(list[i]);
                }
            }
        }
        GetAvailableCraftingOptions();
    }

    public void AddToInventory(Card card){
        SelectionCard selectionCardScript;
        int initialInvCardSize = inventoryCards.Count;
        for(int i = 0; i < initialInvCardSize; i++)
        {
            selectionCardScript = inventoryCards[i].GetComponent<SelectionCard>();
            if (selectionCardScript.card.cardName == card.cardName)
            {
                selectionCardScript.qty += 1;
                selectionCardScript.Render();
                return;
            } 
        }
        GameObject invCard = GameObject.Instantiate(compactPrefab, Vector2.zero, Quaternion.identity);
        invCard.transform.SetParent(selectionArea.transform);
        selectionCardScript = invCard.GetComponent<SelectionCard>();
        selectionCardScript.qty = 1;
        selectionCardScript.card = card;
        selectionCardScript.Render();
        selectionCardScript.isInstantiated = true;
        inventoryCards.Add(invCard);
    }

    public void AddToTable(CraftCard cardScript){

        TableCard tableCardScript;

        for(int i = 0; i < tableCards.Count; i++)
        {
            if (tableCards[i].GetComponent<CraftCard>().card.cardName == cardScript.card.cardName)
            {
                tableCardScript = tableCards[i].GetComponent<TableCard>();
                tableCardScript.qty += 1;
                tableCardScript.Render();
                return;
            } 
        }

        GameObject tableCard = InstantiateCard("table");
        tableCardScript = tableCard.GetComponent<TableCard>();
        tableCardScript.qty = 1;
        tableCardScript.card = cardScript.card;
        tableCardScript.Render();
        tableCard.transform.localScale *= 0.5f;
        cardScript.isInstantiated = true;
        tableCardScript.isInstantiated = true;
        tableCards.Add(tableCard);
    }

    public GameObject InstantiateCard(string location)
    {
        GameObject prefab = null;
        GameObject parent = null;
        switch(location)
        {
        case "inventory":
            prefab = compactPrefab;
            parent = selectionArea;
            break;
            case "table":
            prefab = cardVisualPrefab;
            parent = tableArea;
            break;
        }
        GameObject newSelectionCard = GameObject.Instantiate(prefab, new Vector2(0,0), Quaternion.identity) as GameObject;
        newSelectionCard.transform.SetParent(parent.transform);
        return newSelectionCard;
    }

    public void ClearAll()
    {
        // Clear inventory cards
        foreach (GameObject card in inventoryCards)
        {
            Destroy(card);
        }
        inventoryCards.Clear();

        // Clear table cards
        foreach (GameObject card in tableCards)
        {
            Destroy(card);
        }
        tableCards.Clear();

        // Destroy result card
        if (resultCard != null)
        {
            Destroy(resultCard.gameObject);
            resultCard = null; 
        }
        
    }

        public void RenderResultCard(CraftingRecipe recipe)
    {
        GameObject resultCardInstance = GameObject.Instantiate(cardVisualPrefab, new Vector2(0,0), Quaternion.identity) as GameObject;
        resultCardInstance.tag = "Result Card";
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
        // if(!isRecipeView)
        // {
        bool canCraft = false;
        foreach(CraftingMaterial material in recipe.craftingMaterials){
            canCraft = false;
            foreach(GameObject cardObject in tableCards)
            {
                CraftCard craftCard = cardObject.GetComponent<CraftCard>();
                if(material.key == craftCard.card.cardName){
                    if(craftCard.qty < material.amount)
                    {
                        return false;
                    }
                    else
                    {
                        canCraft = true;
                    }
                }
            }
            if(canCraft == false){
                return false;
            }
        } 
        return canCraft;
        // }
        // else
        // {
        //     return recipe.craftingMaterials.All(material => 
        //     inventory.ContainsKey(material.key) && inventory[material.key] >= material.amount);
        // }
    }

    public bool HasExtraMaterial(CraftingRecipe recipe){
        bool matched = false;
        foreach(GameObject cardObject in tableCards){
            matched = false;
            CraftCard craftCard = cardObject.GetComponent<CraftCard>();
            foreach(CraftingMaterial material in recipe.craftingMaterials){
                if(matched == true){
                    if(craftCard.card.cardName == material.key){
                    matched = true;
                    }
                }
                else{
                    if(craftCard.card.cardName == material.key){
                        matched = true;
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
        CraftCard craftCard;
        foreach(CraftingMaterial material in recipe.craftingMaterials){
            foreach(GameObject cardObject in tableCards){
                craftCard = cardObject.GetComponent<CraftCard>();
                if(material.key == craftCard.card.cardName){
                    tempAmount = craftCard.qty/material.amount; 
                }
                if(tempAmount < amountToCraft){
                    amountToCraft = tempAmount;
                }
            }
        }
        return amountToCraft;    
    }

    //Let's make this thing
    public void CraftItem() {
        Card card = resultCard.GetComponent<CardBehavior>().card;
        int quantityToMake = card.quantity;
        for(int i = 0; i < quantityToMake; i++)
        {
            AddToInventory(card);
            singleton.playerDeck.Add(card);
        }
        UseMaterials();
        singleton.AdjustDaylight(0);
        card.quantity = 0;
        GetAvailableCraftingOptions();
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
            foreach (CraftingRecipe recipe in recipeDatabase) {
                if (recipe.resultItem.cardName == cardBehavior.card.cardName) {
                    return recipe;
                }
            }

        return null;
    }

    //Either reduce qty or remove card objects from crafting table
    private void UpdateTableMaterial(CraftingMaterial material) {
        for (int i = 0; i < tableCards.Count; i++) {
            CraftCard craftCard = tableCards[i].GetComponent<CraftCard>();

            if (material.key == craftCard.card.cardName) {
                craftCard.qty -= material.amount;
                int newQty = craftCard.qty;

                if (newQty == 0) {
                    craftCard.qty = 0;
                    craftCard.Render();
                    Destroy(tableCards[i].gameObject);
                    tableCards.Remove(tableCards[i]);
                } 
                else 
                {
                    craftCard.qty = newQty;
                    craftCard.Render();
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

    public void PossibleRecipeView()
    {
      
        if(!isRecipeView)
        {
            ClearAll();
            isRecipeView = true;
            int initialInvCount = inventoryCards.Count;
            for(int i = 0; i < initialInvCount; i++)
            {
                Destroy(inventoryCards[i].gameObject);
            }

            //Go through recipes
            foreach(CraftingRecipe recipe in recipeDatabase)
            {
                    AddToInventory(recipe.resultItem);
            }
        }
    }
}










