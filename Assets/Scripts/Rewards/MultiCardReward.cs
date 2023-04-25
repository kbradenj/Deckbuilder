using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MultiCardReward : Reward
{
    
    public List<Card> multiRewards;
    public List<GameObject> multiRewardObjects;
    public GameObject cardPrefab;

    protected override void Awake()
    {
        base.Awake();
        for(int i = 0; i < 3; i++)
        {
            Card randomCard = GetRandomCard(singleton.player.level, singleton.player.level, true);
            multiRewards.Add(randomCard);
        }
        foreach(Card card in multiRewards){
            GameObject newCard = GameObject.Instantiate(cardPrefab, new Vector2(0,0), Quaternion.identity);
            newCard.transform.SetParent(gameObject.transform);
            newCard.GetComponent<CardBehavior>().RenderCard(card);
            multiRewardObjects.Add(newCard);
        }

    }
    public override void ConfirmReward()
    {
        base.ConfirmReward();
        foreach(Card card in multiRewards){
            singleton.playerDeck.Add(card);
       }
    }


    public override void Highlight()
    {
        base.Highlight();
        foreach(GameObject cardObject in multiRewardObjects)
        {
            Image cardBGImage = cardObject.transform.Find("CardBG").GetComponent<Image>();
            defaultBGColor = cardBGImage.color;
            cardBGImage.color = highlightColor;
        }
    }


    public override void StopHighlight()
    {
        base.StopHighlight();
        foreach(GameObject cardObject in multiRewardObjects)
        {
            Image cardBGImage = cardObject.transform.Find("CardBG").GetComponent<Image>();
            cardBGImage.color = defaultBGColor;
        }
    }
}
