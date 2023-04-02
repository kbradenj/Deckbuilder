using System.Collections;
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

    //Containers
    public int storeLevel;
    public int dayCount = 1;
    public int maxDaylight = 360;
    public int dayLeft;

    //Dictionaries
    public Dictionary<string, TalentOption> talents = new Dictionary<string, TalentOption>();
    public Dictionary<string, Card> cardLookup;
    public Dictionary<int, Dictionary<int, Dictionary<int, Card>>> cardDictionary;

    //Arrays
    public CraftingRecipe[] recipeDatabase;

    //Lists
    public List<Card> cardDatabase;
    public List<Card> playerDeck;
    public List<CraftingRecipe> unlockedRecipes;

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
            dayLeft = maxDaylight;
            AdjustDaylight();
            player.maxHealth = 100;
            player.level = 1;
            player.baseStrength = 3;
            player.health = player.maxHealth;
            PlayerPrefs.SetString("unlockedRecipes", "");
            recipeDatabase = Resources.LoadAll<CraftingRecipe>("Craft Recipes");
            DontDestroyOnLoad(gameObject);
        }    
        else
        {
            Destroy(gameObject);
        }
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
}
