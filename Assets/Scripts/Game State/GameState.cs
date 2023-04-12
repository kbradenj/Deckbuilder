using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameState : MonoBehaviour
{
    //Game Objects

    public GameObject recipeBookPrefab;

    //Singleton
    public Singleton singleton;

    //Scripts
    public CardManager cardManager;
    public QuestManager questManager;
    public Unlocks unlocks;

    //Dictionaries
    public Dictionary<int, Dictionary<int, Dictionary<int, Card>>> cardDictionary;
    public Dictionary<string, Card> cardLookup;
    public Dictionary<string, Dictionary<string, Card>> powerCards = new Dictionary<string, Dictionary<string, Card>>();

    //Bools
    public bool isBattle = false;

    //Resources
    public List<Card> playerDeck;
    public Card[] database;
    public List<Card> cardDatabase = new List<Card>();

    //Static Amounts
    public float uncommonChance = 40;
    public float rareChance = 15;
    public float legendaryChance = 5;
    public float mythicChance = 0.1f;


    void Awake()
    {
        //Get singleton, check to see if the card db is loaded
        singleton = GameObject.FindObjectOfType<Singleton>();
        singleton.ResetPlayerTempStats();
        if(singleton.cardDatabase.Count <= 0){
            LoadCardDatabase();
        }
        CreatePowerCardDictionary();
        questManager = FindObjectOfType<QuestManager>();
        unlocks = GetComponent<Unlocks>();
        unlocks.CheckUnlocks();
    }

    //Pull in card db
    public void LoadCardDatabase()
    {
        database = Resources.LoadAll<Card>("Cards");
          for(int i = 0; i < database.Length; i++)
        {
            cardDatabase.Add(database[i]);
        }
        CreateCardDatabaseLevels();
        CreateCardLookup();
        singleton.cardDatabase = cardDatabase;
        singleton.cardDictionary = cardDictionary;
        singleton.cardLookup = cardLookup;
    }

    public void CreateCardDatabaseLevels(){
        // Create a dictionary to represent the card levels
        cardDictionary = new Dictionary<int, Dictionary<int, Dictionary<int, Card>>>();

        // Loop through each card level and create a dictionary for each level
        for (int i = 1; i <= 10; i++)
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

    public void OpenRecipeBook()
    {
        GameObject recipeBook = GameObject.Instantiate(recipeBookPrefab, new Vector2(Screen.width/2f, Screen.height/2f), Quaternion.identity);
        recipeBook.transform.SetParent(GameObject.Find("Main Canvas").transform);
    }
}




