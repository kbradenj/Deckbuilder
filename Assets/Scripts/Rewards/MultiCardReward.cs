using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MultiCardReward : Reward
{
    
    public List<Card> multiRewards;
    public GameObject cardPrefab;

    protected override void Start()
    {
        base.Start();
            multiRewards.Add(singleton.cardDatabase[0]);
            multiRewards.Add(singleton.cardDatabase[0]);
            multiRewards.Add(singleton.cardDatabase[0]);
        foreach(Card card in multiRewards){
            GameObject newCard = GameObject.Instantiate(cardPrefab, new Vector2(0,0), Quaternion.identity);
            newCard.transform.SetParent(gameObject.transform);
            newCard.GetComponent<CardBehavior>().RenderCard(card);
        }
    }
    public override void PickReward()
    {
        base.PickReward();
        foreach(Card card in multiRewards){
            singleton.playerDeck.Add(card);
       }
       Destroy(this.gameObject);
    }
}
