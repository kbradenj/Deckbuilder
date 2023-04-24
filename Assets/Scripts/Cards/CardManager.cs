using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CardManager : MonoBehaviour
{
    //Database
    public List<Card> cardDatabase;

    //Singleton
    public Singleton singleton;
    
    //Card Lists
    public List<Card> startingCards = new List<Card>();
    public List<Card> deckCards = new List<Card>();
    public List<Card> handCards = new List<Card>();
    public List<GameObject> handCardObjects = new List<GameObject>();
    public List<Card> discardCards = new List<Card>();
  

    //Dictionaries
    public Dictionary<string, Dictionary<string, Card>> powerCards;

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
        player = singleton.player;
        gameState = GameObject.FindObjectOfType<GameState>();
        hand = GameObject.Find("Hand");
        if(singleton.playerDeck.Count <= 0)
        {
            CreatePlayerDeck();
        }
    }
    
    
    public void CreatePlayerDeck(){
            foreach(Card card in startingCards)
            {
                for(int j = 0; j < 4; j++)
                {
                    deckCards.Add(card);
                }  
            }   
        singleton.playerDeck = deckCards;
    }

    public void LoadPlayerDeck()
    {
        deckCards = new List<Card>(singleton.playerDeck);
        UpdateDeckSizeText();
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
        GridLayoutGroup gridLayout = hand.GetComponentInChildren<GridLayoutGroup>();
        RectTransform handRect = hand.GetComponent<RectTransform>();
        float overlap = (handRect.rect.width - ((amount + handCardObjects.Count) * 300))/(amount + handCardObjects.Count);
        gridLayout.spacing = new Vector2 (overlap, 0);

        if(deckCards.Count < amount){
           Shuffle();
        }
        for(int i = 0; i < amount; i++)
        {
            if(deckCards.Count == 0)
             {

                //If deck is still empty after shuffle
                return;
            }
            else
            {
                //Instantiate card
                GameObject tempCard = GameObject.Instantiate(cardGameObject, new Vector2(0,0), Quaternion.identity) as GameObject;
                CardBehavior cardBehavior = tempCard.GetComponent<CardBehavior>();

                // Randomize deal
                int rand = UnityEngine.Random.Range(0, deckCards.Count);
                cardBehavior.card = deckCards[rand];


                // Display card in UI
                cardBehavior.RenderCard(cardBehavior.card);

                // Move cards from deck to hand
                handCards.Add(cardBehavior.card);
                handCardObjects.Add(tempCard);
                
                //Remove Cards from deck
                deckCards.Remove(cardBehavior.card);
                UpdateDeckSizeText();

                // Tag hand cards for easy destroy on end turn
                tempCard.tag = "Hand Card";
                tempCard.transform.SetParent(hand.transform, false); 
            }   
        }
    }   

    public void UpdateHandCards()
    {
        foreach(GameObject card in handCardObjects)
        {
            CardBehavior cardBehavior = card.GetComponent<CardBehavior>();
            Card handCard = cardBehavior.card;
            handCard.modDamage = (int) Math.Floor((handCard.attack * player.weaknessMod) + player.attackBoost + player.strength + player.baseStrength);
            cardBehavior.descriptionField.text = handCard.FormatString();
        }
    }

    public void ShriekCards(int amount)
    {
        int disabledCount = 0;
         for (int i = 0; i < amount; i++)
            {

                int counter = 0;
                while(counter < 100 && disabledCount < 2)
                {
                    counter++;
                    int randomNum = UnityEngine.Random.Range(0,handCardObjects.Count);
                    CardBehavior cardBehavior = handCardObjects[randomNum].GetComponent<CardBehavior>();
                    if(!cardBehavior.isDisabled)
                    {
                        DisableCard(cardBehavior);
                        disabledCount++;
                    }
                }
              
            }
    }

    public void DisableCard(CardBehavior cardBehavior)
    {
        cardBehavior.isDisabled = true;
        cardBehavior.RenderCard(cardBehavior.card, false);
    }

    public void Discard()
    {
        // Destroy hand game objects
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Hand Card");
        foreach(GameObject card in cards)
        {
            Destroy(card);
        }
        handCardObjects.Clear();
        // Move cards from hand to discard
        int intitialHandSize = handCards.Count;
        for(int i = 0; i < intitialHandSize; i++)
        {
            MoveFromHandToDiscard(handCards[0]);
        }

        UpdateDeckSizeText();
    }

    public void MoveFromHandToDiscard(Card card)
    {
        discardCards.Add(card);
        handCards.Remove(card);
    }
   
    public void Shuffle(){
        int initialDiscardSize = discardCards.Count;
        for(int i = 0; i < initialDiscardSize; i++){
            int randIndex = UnityEngine.Random.Range(0, discardCards.Count);
            deckCards.Add(discardCards[randIndex]);
            discardCards.Remove(discardCards[randIndex]);
        }
    }
}
