using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MultiCardReward : Reward
{
    
    public List<Card> multiRewards;
    public GameObject cardPrefab;

    protected override void Awake()
    {
        base.Awake();
        for(int i = 0; i < 3; i++)
        {
            multiRewards.Add(GetRandomCard(singleton.player.level, singleton.player.level, true));
        }
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
    }
}
