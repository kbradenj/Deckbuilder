using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{

    public List<Card> playerDeck;
    public Player player;
    public List<Card> cardDatabase;

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
            dayLeft = 360;
            player.maxHealth = 100;
            player.level = 1;
            player.health = player.maxHealth;
            DontDestroyOnLoad(gameObject);
        }    
        else
        {
            Destroy(gameObject);
        }
        
    }

    public void RemoveCardFromDeck(string cardName){
        Debug.Log(cardName);
        playerDeck.Remove(playerDeck.Find(item => item.cardName == cardName));
    }

    public void AdjustDaylight(int amount)
    {
        if(dayLeft - amount > 0)
        {
            dayLeft -= amount;
        }
        else
        {
            Debug.Log("Not enough time left in the day");
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
