using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{

    public List<Card> playerDeck;
    public Player player;
    public List<Card> cardDatabase;

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
            DontDestroyOnLoad(gameObject);
        }    
        else
        {
            Destroy(gameObject);
        }
        
    }
}
