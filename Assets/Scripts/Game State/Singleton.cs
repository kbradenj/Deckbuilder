using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Singleton : MonoBehaviour
{

    public List<Card> playerDeck;
    public Player player;
    public List<Card> cardDatabase;

    public Navigation navigation;

    public int storeLevel;
    public int maxDaylight = 360;

    public Dictionary<int, Dictionary<int, Dictionary<int, Card>>> cardDictionary;
    public int dayLeft;

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
            player.maxHealth = 100;
            player.level = 1;
            player.baseStrength = 3;
            player.health = player.maxHealth;
            DontDestroyOnLoad(gameObject);
        }    
        else
        {
            Destroy(gameObject);
        }
    }

    public void RemoveCardFromDeck(string cardName){
        playerDeck.Remove(playerDeck.Find(item => item.cardName == cardName));
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
                Debug.Log("Begin Night");
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
