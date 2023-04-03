using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Craft : MonoBehaviour
{

    public Dictionary<string, Card> playerDeck = new Dictionary<string, Card>();

    //Singleton
    private Singleton singleton;

    //Lists
    private List<GameObject> ingredientObjects = new List<GameObject>();
    private List<GameObject> recipeListItems = new List<GameObject>();

    //Areas
    private GameObject recipeList;
    private GameObject resultCardArea;
    private GameObject ingredientsArea;

    //Prefabs
    public GameObject recipeTextPrefab;
    public GameObject cardVisualPrefab;
    public GameObject ingredientPrefab;

    //UI
    public TMP_Text recipeTitle;
    public TMP_Text recipeDescription;
    public TMP_Text costText;
    public Color disabledTextColor;
    public Color defaultTextColor;

    //SelectedRecipe
    public CraftingRecipe currentRecipe = null;
    public GameObject currentRecipeListItem;
    public GameObject currentRecipeResultItem;
    
     void Start()
    {
        if(SceneManager.GetActiveScene().name == "Crafting")
        {
            GameObject.Find("Close Button").SetActive(false);
        }
        else
        {
            GameObject.Find("Craft Button").SetActive(false);
        }

        resultCardArea = GameObject.Find("Result Card");
        recipeList = GameObject.Find("Recipe List");
        ingredientsArea = GameObject.Find("Ingredients");
        singleton = GameObject.FindObjectOfType<Singleton>();
        singleton.AdjustDaylight();
        LoadPlayerDeckDictionary();
        LoadRecipes();
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

    public void LoadRecipes()
    {
        foreach(CraftingRecipe recipe in singleton.recipeDatabase)
        {
            GameObject recipeObject = GameObject.Instantiate(recipeTextPrefab, Vector2.zero, Quaternion.identity);
            TMP_Text recipeText = recipeObject.GetComponent<TMP_Text>();
            RecipeBookItem recipeSelectionScript = recipeObject.GetComponent<RecipeBookItem>();
            recipeSelectionScript.recipe = recipe;
            recipeListItems.Add(recipeObject);
            if(recipe.isLocked)
            {
                 recipeText.text = "?????????";
                 recipeText.color = disabledTextColor;
            }
            else
            {
                recipeText.text = recipe.resultItem.cardName;
                if(CanCraft(recipe))
                {
                    recipeText.color = defaultTextColor;
                }
                else
                {
                    recipeText.color = disabledTextColor;
                }
            }
            recipeObject.transform.SetParent(recipeList.transform);
        }
    }

    public void StillCraftable()
    {
        foreach(GameObject item in recipeListItems)
        {
           RecipeBookItem itemScript = item.GetComponent<RecipeBookItem>();
           if(CanCraft(itemScript.recipe))
           {
            continue;
           }
           else
           {
            itemScript.GetComponent<TMP_Text>().color = disabledTextColor;
           }
        }
    }

        public void Unlock()
    {
        singleton.unlockedRecipes.Add(singleton.recipeDatabase[0]);
        singleton.recipeDatabase[0].isLocked = false;
        LoadRecipes();
    }

    public void ShowRecipe(CraftingRecipe recipe)
    {
        int ownedQty;
        Card materialCard = null;

        if(currentRecipe != null)
        {
            ClearRecipe();
        }
        currentRecipe = recipe;
        recipeTitle.text = recipe.resultItem.cardName;
        recipeDescription.text = recipe.resultItem.FormatString();
        GameObject resultCard = GameObject.Instantiate(cardVisualPrefab, Vector2.zero, Quaternion.identity);
        currentRecipeResultItem = resultCard;
        resultCard.GetComponent<CardBehavior>().RenderCard(recipe.resultItem);
        resultCard.transform.SetParent(resultCardArea.transform);
        resultCard.transform.localScale = new Vector2 (.8f, .8f);
        costText.text = "Cost: " + recipe.timeCost + " min";

        foreach(CraftingMaterial material in recipe.craftingMaterials)
        {
            GameObject ingredientItemObj = GameObject.Instantiate(ingredientPrefab, Vector2.zero, Quaternion.identity);
            IngredientItem ingredientItemScript = ingredientItemObj.GetComponent<IngredientItem>();
            ingredientObjects.Add(ingredientItemObj);
            ingredientItemObj.transform.SetParent(ingredientsArea.transform);

            if(playerDeck.ContainsKey(material.key)){
                ownedQty = playerDeck[material.key].deckQty;
            }
            else
            {
                ownedQty = 0;
            }

            foreach(Card card in singleton.cardDatabase)
            {
                if(card.cardName == material.key)
                {
                    materialCard = card;
                }
            }
            ingredientItemScript.Render(materialCard, material.amount, ownedQty);
        }
    }

    public void ClearRecipe()
    {
        Destroy(currentRecipeResultItem);
        currentRecipe = null;
        foreach(GameObject ingredient in ingredientObjects)
        {
            Destroy(ingredient);
        }
    }

    public void CraftCard()
    {
        if(!CanCraft(currentRecipe) || singleton.dayLeft < currentRecipe.timeCost)
        {
            Debug.Log("Oops, Can't Craft");
        }
        else
        {
            singleton.dayLeft -= currentRecipe.timeCost;
            singleton.AdjustDaylight();
            AddCardToDeck(currentRecipe.resultItem.cardName);   
            RemoveIngredientsFromDeck();
        }
        StillCraftable();
    }

    private void AddCardToDeck(string cardName)
    {
        singleton.playerDeck.Add(singleton.cardLookup[cardName]);
    }

    public void RemoveIngredientsFromDeck()
    {
         Debug.Log( singleton.playerDeck.Count);
        foreach(CraftingMaterial material in currentRecipe.craftingMaterials)
        {
            for(int i = 0; i < material.amount; i++)
            {
                singleton.RemoveCardFromDeck(material.key);
                playerDeck[material.key].deckQty--;
            }
        
        }
        ShowRecipe(currentRecipe);
    }

    public bool CanCraft(CraftingRecipe recipe)
    {
        foreach(CraftingMaterial material in recipe.craftingMaterials)
        {
            if(playerDeck.ContainsKey(material.key))
            {
                if(playerDeck[material.key].deckQty >= material.amount)
                {

                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public void Close()
    {
        Destroy(gameObject);
    }


}
