using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardReward : Reward
{
    public Card card;
    public GameObject cardPrefab;
    public Dictionary<int, Dictionary<int, Card>> rewardOptions;
    


    protected override void Awake()
    {
        base.Awake();
        int playerLevel = singleton.player.level;
        Dictionary<int, Dictionary<int, Card>> levelDictionary = singleton.cardDictionary[playerLevel];

        int randomRarity = Random.Range(1, levelDictionary.Count+1);
        int cardCount=0;
        
        while(cardCount == 0)
        {
            cardCount = singleton.cardDictionary[playerLevel][randomRarity].Count;
            if(cardCount > 0)
            {
                break;
            }
        }
        
        card = singleton.cardDictionary[playerLevel][randomRarity][Random.Range(0,cardCount)];
        Debug.Log(card.cardName);
        GameObject newCard = GameObject.Instantiate(cardPrefab, new Vector2(0,0), Quaternion.identity);
        newCard.transform.SetParent(gameObject.transform);
        newCard.GetComponent<CardBehavior>().RenderCard(card);
    }
    public override void PickReward()
    {
        base.PickReward();
        singleton.playerDeck.Add(card);
    }

}
