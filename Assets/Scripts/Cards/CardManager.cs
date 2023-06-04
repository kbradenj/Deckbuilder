using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

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

    //Reference Data
    public float cardWidth = 0;
    public Vector2 cardScale = new Vector2(.75f, .75f);

    //States
    public bool isDraggingGlobal = false;

    void Start()
    {
        singleton = GameObject.FindObjectOfType<Singleton>();
        player = singleton.player;
        gameState = GameObject.FindObjectOfType<GameState>();
        hand = GameObject.Find("Hand");
        cardWidth = cardGameObject.GetComponent<RectTransform>().sizeDelta.x * cardScale.x;
        if(singleton.playerDeck.Count <= 0)
        {
            CreatePlayerDeck();
        }
    }
    
    
    public void CreatePlayerDeck(){
            foreach(Card card in startingCards)
            {
                deckCards.Add(card);
            }   
        singleton.playerDeck = deckCards;
    }

    public void LoadPlayerDeck()
    {
        Debug.Log("Player Deck Loaded");
        deckCards = new List<Card>(singleton.playerDeck);
        UpdateDeckSizeText();
    }

    //UI Text Updates
    public void UpdateDeckSizeText()
    {   if(singleton.isBattle)
        {  
            deckSize = GameObject.Find("Deck Size").GetComponent<TMP_Text>();
            deckSize.text = deckCards.Count.ToString();
            discardSize = GameObject.Find("Discard Size").GetComponent<TMP_Text>();
            discardSize.text = discardCards.Count.ToString();
        }
    }

    //Draw, Discard, Shuffle
    public void Draw(int amount, bool isStartTurn = false)
    {
        if(deckCards.Count < amount && isStartTurn){
           Shuffle();
        }
        for(int i = 0; i < amount; i++)
        {
            if(deckCards.Count == 0)
             {
                break;
            }
            else
            {
                //Instantiate card
                GameObject tempCard = GameObject.Instantiate(cardGameObject, new Vector2(-800 - (cardWidth * i),0), Quaternion.identity) as GameObject;
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
                tempCard.transform.localScale = cardScale;
            }   
        }

        RotateCards();
        PositionCards();
                    //    Debug.Log("Deck Card Size after draw " + deckCards.Count);
                    //    Debug.Log("Hand Card Size after draw " + handCards.Count);
                    //    Debug.Log("Discard Card Size after draw " + discardCards.Count);
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

    public void RotateCards()
    {
     
        int counter = -20;
        foreach(GameObject card in handCardObjects)
        {
            if(handCardObjects.Count < 6)
            {
                card.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                card.transform.eulerAngles = new Vector3(0, 0, counter);
                counter += 40 / handCardObjects.Count;
            }
        }
    }

    public void PositionCards(bool fast = false){
        float animationSpeed = .75f;
        if(fast)
        {
            animationSpeed = .3f;
        }

        RectTransform handRect = hand.GetComponent<RectTransform>();

        int numOfHandCards = handCardObjects.Count;
        float widthOfHandArea = handRect.rect.width;
        float widthOfHandCards = numOfHandCards * cardWidth;
        float overlap = (widthOfHandArea + (numOfHandCards * cardWidth))/numOfHandCards;

        float arcHeight = Math.Clamp(10 * numOfHandCards, 0, 100);
        float arcRadius = 8000 / numOfHandCards;
        float angleStep = 180f / (numOfHandCards - 1);

        int middleIndex = numOfHandCards / 2;
        
        for (int i = 0; i < numOfHandCards; i++)
        {
            float angle;
             if (numOfHandCards % 2 == 0)
            {
                angle = angleStep * (i - middleIndex + 0.5f); 
            }
            else
            {
                angle = angleStep * (i - middleIndex);
            }

            float spacing;
            if (numOfHandCards % 2 == 0)
            {
                spacing = i - middleIndex + 0.5f; 
            }
            else
            {
                spacing = i - middleIndex;
            }

            float y = Mathf.Cos(angle * Mathf.Deg2Rad) * arcHeight - 100f;
            
            if(numOfHandCards < 6)
            {
                y = -50f;
                overlap = cardWidth * 2;
            }
       
            Vector2 newCardPosition = new Vector2(spacing * (cardWidth - overlap), y);
            Vector2 worldPos = (Vector2)newCardPosition + (Vector2)hand.transform.position;

            CardBehavior cardBehavior = handCardObjects[i].GetComponent<CardBehavior>();
            handCardObjects[i].transform.DOMove(new Vector3(worldPos.x, worldPos.y, 0f), animationSpeed);
         

            cardBehavior.startPosition = worldPos;
            cardBehavior.startSiblingIndex = handCardObjects[i].transform.GetSiblingIndex();
        }
    }

    public void ShriekCards()
    {
        int disabledCount = 0;
        int shriekAmount = Mathf.Clamp(player.shriek, 0, handCardObjects.Count);
         for (int i = 0; i < shriekAmount; i++)
            {

                int counter = 0;
                while(counter < 100 && disabledCount < shriekAmount)
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

    public void EnableCard(CardBehavior cardBehavior)
    {
        cardBehavior.isDisabled = false;
    }

    public void Discard()
    {
        // Destroy hand game objects
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Hand Card");
        foreach(GameObject card in cards)
        {
            EnableCard(card.GetComponent<CardBehavior>());
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
