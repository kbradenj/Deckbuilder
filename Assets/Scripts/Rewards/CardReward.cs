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
        card = GetRandomCard(singleton.player.level+1, singleton.player.level+1, false);
        GameObject newCard = GameObject.Instantiate(cardPrefab, new Vector2(0,0), Quaternion.identity);
        newCard.transform.SetParent(gameObject.transform);
        newCard.GetComponent<CardBehavior>().RenderCard(card);
         
    }
    public override void ConfirmReward()
    {
        base.ConfirmReward();
        singleton.playerDeck.Add(card);
    }

}
