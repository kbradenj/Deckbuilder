using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{

    public List<Card> playerDeck;
    public Player player;
    public List<Card> cardDatabase;
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
}
