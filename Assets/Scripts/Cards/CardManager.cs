using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CardManager : MonoBehaviour
{
    //Database
    public List<Card> cardDatabase;

    //Singleton
    public Singleton singleton;
    
    //Card Lists
    public List<int> startingCardIDs = new List<int>();
    public List<Card> deckCards = new List<Card>();
    public List<Card> handCards = new List<Card>();
    public List<Card> discardCards = new List<Card>();

    //TMP
    public TMP_Text deckSize;
    public TMP_Text discardSize;

    //Scripts
    public Card[] database;
    public Card card;
    public Player player;

    //Game Objects
    public GameObject cardGameObject;
    public GameObject hand;
    public GameState gameState;

    void Awake()
    {
        singleton = GameObject.FindObjectOfType<Singleton>();
        gameState = GameObject.FindObjectOfType<GameState>();
    }
  
    public void CreatePlayerDeck(){
        cardDatabase = gameState.cardDatabase;
        
        startingCardIDs.Add(0);
        startingCardIDs.Add(1);
        startingCardIDs.Add(2);
        startingCardIDs.Add(3);

        for(int i = 0; i < startingCardIDs.Count; i++)
        {
            Card startingCard = cardDatabase[startingCardIDs[i]];
            for(int j = 0; j < 5; j++)
            {
                deckCards.Add(startingCard);
            }  
        }
        singleton.playerDeck = deckCards;
    }

    public void LoadPlayerDeck(Player player)
    {
        deckCards = new List<Card>(singleton.playerDeck);
        if(SceneManager.GetActiveScene().name == "Battle"){
            UpdateDeckSizeText();
            hand = GameObject.Find("Hand");
            Draw(player.drawSize);
        }
    }

    //UI Text Updates
    public void UpdateDeckSizeText()
    {   if(gameState.isBattle)
        {  
            deckSize = GameObject.Find("Deck Size").GetComponent<TMP_Text>();
            deckSize.text = deckCards.Count.ToString();
            discardSize = GameObject.Find("Discard Size").GetComponent<TMP_Text>();
            discardSize.text = discardCards.Count.ToString();
        }
    }

    //Draw, Discard, Shuffle
    public void Draw(int amount)
    {
        if(deckCards.Count < amount){
           Shuffle();
        }
        for(int i = 0; i < amount; i++)
        {
            GameObject tempCard = GameObject.Instantiate(cardGameObject, new Vector2(0,0), Quaternion.identity) as GameObject;
            CardBehavior cardBehavior = tempCard.GetComponent<CardBehavior>();

            // Randomize deal
            int rand = Random.Range(0, deckCards.Count);
            card = deckCards[rand];
   
            // Display card in UI
            cardBehavior.RenderCard(card);

            // Move cards from deck to hand
            handCards.Add(card);
            
            deckCards.Remove(card);
            UpdateDeckSizeText();

            // Tag hand cards for easy destroy on end turn
            tempCard.tag = "Hand Card";
            tempCard.transform.SetParent(hand.transform, false); 
        }
    }   

    public void Discard()
    {
        // Destroy hand game objects
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Hand Card");
        foreach(GameObject card in cards)
        {
            Destroy(card);
        }

        // Move cards from hand to discard
        int intitialHandSize = handCards.Count;
        for(int i = 0; i < intitialHandSize; i++)
        {
            Card card = handCards[0];
            discardCards.Add(card);
            handCards.Remove(card);
        }
        UpdateDeckSizeText();
    }
   
    public void Shuffle(){
        int initialDiscardSize = discardCards.Count;
        for(int i = 0; i < initialDiscardSize; i++){
            int randIndex = Random.Range(0, discardCards.Count);
            deckCards.Add(discardCards[randIndex]);
            discardCards.Remove(discardCards[randIndex]);
        }
    }
}
