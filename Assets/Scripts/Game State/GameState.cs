using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class GameState : MonoBehaviour
{
    //Game Objects

    public GameObject recipeBookPrefab;

    //Singleton
    public Singleton singleton;
    public GameObject singletonPrefab;

    //Scripts
    public CardManager cardManager;
    public MilestoneManager milestoneManager;
    public Unlocks unlocks;

    //Dictionaries
    public Dictionary<int, Dictionary<int, Dictionary<int, Card>>> cardDictionary;
    public Dictionary<string, Card> cardLookup;
    public Dictionary<string, Dictionary<string, Card>> powerCards = new Dictionary<string, Dictionary<string, Card>>();
    public Dictionary<string, CraftingRecipe> recipeDictionary = new Dictionary<string, CraftingRecipe>();

    //Bools
    public bool isBattle = false;

    //Resources
    public List<Card> playerDeck;
    public Card[] cardDatabase;
    public CraftingRecipe[] recipeDatabase;
    public Artifact[] artifactDatabase;
    public List<Card> cardList = new List<Card>();

    //Static Amounts
    public float uncommonChance = 40;
    public float rareChance = 15;
    public float legendaryChance = 5;
    public float mythicChance = 0.1f;

    void Awake()
    {
        //Get singleton or make one, check to see if the card db is loaded
        if(GameObject.FindObjectOfType<Singleton>() != null)
        {
            singleton = GameObject.FindObjectOfType<Singleton>();
        }
        else
        {
            CreateSingleton();
        }

        singleton.ResetPlayerTempStats();

        if(singleton.recipeDictionary.Count <= 0)
        {
            LoadRecipeDatabase();
        }

        if(singleton.cardDatabase.Count <= 0){
            LoadCardDatabase();
        }

        if(artifactDatabase.Length == 0)
        {
            CreateArtifactDatabase();
        }

        CreatePowerCardDictionary();
        LoadActiveArtifacts();
        milestoneManager = FindObjectOfType<MilestoneManager>();
        unlocks = GetComponent<Unlocks>();
        unlocks.CheckUnlocks();
    }

    public void CreateSingleton()
    {
        GameObject singletonObject = GameObject.Instantiate(singletonPrefab, Vector2.zero, Quaternion.identity);
        singleton = singletonObject.GetComponent<Singleton>();  
    }

    //Pulls in all created cards in the database
    public void LoadCardDatabase()
    {
        cardDatabase = Resources.LoadAll<Card>("Cards");
          for(int i = 0; i < cardDatabase.Length; i++)
        {
            cardList.Add(cardDatabase[i]);
        }
        CreateCardDatabaseLevels();
        CreateCardLookup();
        singleton.cardDatabase = cardList;
        singleton.cardDictionary = cardDictionary;
        singleton.cardLookup = cardLookup;
    }

    //Creates a nested dictionary first broken out by card level(1-25) then within each card level broken out by rarity (1-4)
    public void CreateCardDatabaseLevels(){
        cardDictionary = new Dictionary<int, Dictionary<int, Dictionary<int, Card>>>();

        // Loop through each card level and create a dictionary for each level
        for (int i = 1; i <= 25; i++)
        {
            Dictionary<int, Dictionary<int, Card>> rarityDictionary = new Dictionary<int, Dictionary<int, Card>>();

            // Loop through each rarity level and create a dictionary for each rarity
            for (int j = 1; j <= 4; j++)
            {
                Dictionary<int, Card> cards = new Dictionary<int, Card>();
                int tempCount = 0;
                foreach(Card card in cardDatabase){
                    if(card.cardLevel == i && card.cardRarity == j){
                        cards.Add(tempCount, card);
                        tempCount++;
                    }
                }
                rarityDictionary.Add(j, cards);
            }
            cardDictionary.Add(i, rarityDictionary);
        }
    }

    //Creates an easy to use dictionary to look up using card name
    public void CreateCardLookup()
    {
        cardLookup = new Dictionary<string, Card>();
        foreach(Card card in cardDatabase)
        {
            cardLookup.Add(card.cardName, card);
        }
    }

    public void CreatePowerCardDictionary()
    {
        Dictionary<string, Card> turnStart = new Dictionary<string, Card>();
        powerCards.Add("turnStart", turnStart);
        Dictionary<string, Card> turnEnd = new Dictionary<string, Card>();
        powerCards.Add("turnEnd", turnEnd);
        Dictionary<string, Card> battleEnd = new Dictionary<string, Card>();
        powerCards.Add("battleEnd", battleEnd);
    }

    public void CreateArtifactDatabase()
    {
        artifactDatabase = Resources.LoadAll<Artifact>("Artifacts");
    }

    public void LoadActiveArtifacts()
    {
        foreach(Artifact artifact in artifactDatabase)
        {
            singleton.activeArtifacts = artifactDatabase
            .Where(a => a.isActive)
            .ToList();
            Debug.Log(artifact);
        }
        
    }

    public void LoadRecipeDatabase()
    {
        recipeDatabase = Resources.LoadAll<CraftingRecipe>("Craft Recipes");
        CreateRecipeDictionary();
    }

    public void CreateRecipeDictionary()
    {
        foreach(CraftingRecipe recipe in recipeDatabase)
        {
            recipeDictionary.Add(recipe.resultItem.cardName, recipe);
        }
        singleton.recipeDictionary = recipeDictionary;
    }

    public void OpenRecipeBook()
    {
        GameObject recipeBook = GameObject.Instantiate(recipeBookPrefab, new Vector2(Screen.width/2f, Screen.height/2f), Quaternion.identity);
        recipeBook.transform.SetParent(GameObject.Find("Main Canvas").transform);
    }

    public bool MouseIsOnScreen(){
        if (Input.mousePosition.x == 0 || Input.mousePosition.y == 0 || Input.mousePosition.x >= Handles.GetMainGameViewSize().x - 1 || Input.mousePosition.y >= Handles.GetMainGameViewSize().y - 1){
        return false;
        }

        if (Input.mousePosition.x == 0 || Input.mousePosition.y == 0 || Input.mousePosition.x >= Screen.width - 1 || Input.mousePosition.y >= Screen.height - 1) {
        return false;
        }

        else {
            return true;
        }
    }
}




