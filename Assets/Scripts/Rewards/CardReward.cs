using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardReward : Reward
{
    public Card card;
    public GameObject cardPrefab;


    protected override void Start()
    {
        base.Start();
        card = singleton.cardDatabase[5];
        GameObject newCard = GameObject.Instantiate(cardPrefab, new Vector2(0,0), Quaternion.identity);
        newCard.transform.SetParent(gameObject.transform);
        newCard.GetComponent<CardBehavior>().RenderCard(card);
    }
    public override void PickReward()
    {
        base.PickReward();
        singleton.playerDeck.Add(card);
        Destroy(gameObject);
    }

}
