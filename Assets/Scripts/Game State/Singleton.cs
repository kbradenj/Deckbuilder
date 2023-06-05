using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Singleton : MonoBehaviour
{

    //Character
    public Player player;
    
    //Navigation
    public Navigation navigation;

    //Unlocks
    public Unlocks unlocks;

    //Containers
    public int storeLevel;
    public int dayCount = 1;
    public int maxDaylight = 360;
    public int maxMoonlight = 240;
    public int nightLeft;
    public int dayLeft;
    public PathChoice currentPathChoice;

    //Dictionaries
    public Dictionary<string, TalentOption> talents = new Dictionary<string, TalentOption>();
    public Dictionary<string, Card> cardLookup;
    public Dictionary<int, Dictionary<int, Dictionary<int, Card>>> cardDictionary;
    public Dictionary<int, PathChoice> pathChoices = new Dictionary<int, PathChoice>();
    public Dictionary<int, Dictionary<string, List<EnemyObject>>> enemyCatalog = new Dictionary<int, Dictionary<string, List<EnemyObject>>>();
    public Dictionary<int, List<Milestone>> milestoneCatalog = new Dictionary<int, List<Milestone>>();
    public Dictionary<string, CraftingRecipe> recipeDictionary = new Dictionary<string, CraftingRecipe>();

    //Arrays
    public EnemyObject[] enemyDatabase;

    //Lists
    public List<Card> cardDatabase;
    public List<Card> playerDeck;
    public List<CraftingRecipe> unlockedRecipes;
    public List<Artifact> activeArtifacts;
    

    //Bools
    public bool isNight = false;
    public bool isBattle = false;

    //Singleton Pattern
    private static Singleton instance;
    public static Singleton Instance
    {
        get{return instance;}
    }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            if(player == null)
            {
                player = gameObject.AddComponent<Player>();
            }
            DontDestroyOnLoad(gameObject);
            dayLeft = maxDaylight;
            nightLeft = maxMoonlight;
            AdjustDaylight();
            InitializePlayerStats();
            ResetPlayerTempStats();
            unlocks = FindAnyObjectByType<Unlocks>();
            PlayerPrefs.SetString("unlockedRecipes", "");
        }    
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitializePlayerStats()
    {
        player.maxHealth = 100;
        player.level = 1;
        player.health = player.maxHealth;
        player.weaknessMod = 1;
        player.vulnerableMod = 1;
        player.baseStrength = 0;
        player.baseDefense = 0;
        player.drawSize = 5;

    }

    public void ResetPlayerTempStats()
    {
        player.attackBoost = 0;
        player.weak = 0;
        player.fear = 0;
        player.vulnerable = 0;
        player.bonusDraw = 0;
        player.strength = player.baseStrength;
        player.defense = player.baseDefense;
        player.shriek = 0;
        player.evade = 0;
        player.damageReduction = 0;
    }

    public void RemoveCardFromDeck(string cardName){
        playerDeck.Remove(playerDeck.First(card => card.cardName == cardName));
    }

    public void AdjustDaylight(int amount = 0)
    {
        if(amount != 0){
            if(dayLeft - amount > 0)
            {
                dayLeft -= amount;
            }
            else if(dayLeft - amount == 0)
            {
                dayLeft -= amount;
                navigation.Night();
            }
            else
            {
                Debug.Log("Not Enough time in the day");
            }
        }

        if(GameObject.Find("Daylight Counter") != null){
            TMP_Text daylightCounter = GameObject.Find("Daylight Counter").GetComponentInChildren<TMP_Text>();
            daylightCounter.text = dayLeft.ToString() + " minutes left in the day";
        }
    }

     public void AdjustMoonlight(int amount = 0)
    {
        nightLeft -= amount;
        if(GameObject.Find("Moonlight Counter") != null){
            TMP_Text moonlightCounter = GameObject.Find("Moonlight Counter").GetComponentInChildren<TMP_Text>();
            moonlightCounter.text = nightLeft.ToString() + " minutes to sunrise";
        }
    }

    public void UpdateDayCount()
    {
        TMP_Text dayCounter = GameObject.Find("Day Counter").GetComponentInChildren<TMP_Text>();
        dayCounter.text = "Day: " + dayCount;
    }

    public void HealPlayer(int amount)
    {
        if(player.health + amount > player.maxHealth)
        {
            player.health = player.maxHealth;
        }
        else
        {
            player.health += amount;
        }
    }

    public bool CanSpendDaylight(int cost)
    {
        if(dayLeft - cost >= 0){
            return true;
        }
        else
        {
            return false;
        }
    }

    public Card GetRandomAvailableCard()
    {
        List<Card> availableCards = new List<Card>();
        foreach(Card card in cardDatabase)
        {
            if(card.isLocked == false)
            {
                availableCards.Add(card);
            }
        }
        int randomIndex = Random.Range(0, availableCards.Count);
        return availableCards[randomIndex];
    }
}
