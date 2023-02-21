using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardManager : MonoBehaviour
{
    //Database
    public List<Card> cardDatabase = new List<Card>();
    
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

    void Awake()
    {
        startingCardIDs.Add(0);
        startingCardIDs.Add(1);
        startingCardIDs.Add(2);
        startingCardIDs.Add(3);
    }

    void Start()
    {
        hand = GameObject.Find("Hand");
        player = GameObject.Find("Player(Clone)").GetComponent<Player>();
    }

    //Loading Assets
    public void LoadCardDatabase()
    {
        database = Resources.LoadAll<Card>("Cards");
        for(int i = 0; i < database.Length; i++)
        {
            cardDatabase.Add(database[i]);
        }
    }

    public void LoadPlayerDeck(Player player)
    {
        for(int i = 0; i < startingCardIDs.Count; i++)
        {
            Card startingCard = cardDatabase[startingCardIDs[i]];
            for(int j = 0; j < 8; j++)
            {
                deckCards.Add(startingCard);
            }  
        }
        UpdateDeckSizeText();
    }

    //UI Text Updates
    public void UpdateDeckSizeText()
    {     
        deckSize.text = deckCards.Count.ToString();
        discardSize.text = discardCards.Count.ToString();
    }

    //Draw, Discard, Shuffle
    public void Draw()
    {
        if(deckCards.Count < player.drawSize){
           Shuffle();
        }

        for(int i = 0; i < player.drawSize; i++)
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
        foreach(GameObject x in cards)
        {
            Destroy(x);
        }

        // Move cards from hand to discard
        for(int i = 0; i < player.drawSize; i++)
        {
            Card x = handCards[0];
            discardCards.Add(card);
            handCards.Remove(card);
        }
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
