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
    public GameObject newCard;

    protected override void Awake()
    {
        base.Awake();
        card = GetRandomCard(singleton.player.level+1, singleton.player.level+1, false);
        newCard = GameObject.Instantiate(cardPrefab, new Vector2(0,0), Quaternion.identity);
        newCard.transform.SetParent(gameObject.transform);
        newCard.GetComponent<CardBehavior>().RenderCard(card);
         
    }
    public override void ConfirmReward()
    {
        base.ConfirmReward();
        singleton.playerDeck.Add(card);
    }

    public override void Highlight()
    {
        base.Highlight();
        Image cardBGImage = newCard.transform.Find("CardBG").GetComponent<Image>();
        defaultBGColor = cardBGImage.color;
        cardBGImage.color = highlightColor;
    }


    public override void StopHighlight()
    {
        base.StopHighlight();
        newCard.transform.Find("CardBG").GetComponent<Image>().color = defaultBGColor;
    }

}
