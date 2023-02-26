using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MultiCardReward : Reward
{
    
    public List<Card> multiRewards;
    public GameObject cardPrefab;

    void Start()
    {
         foreach(Card card in multiRewards){
            GameObject newCard = GameObject.Instantiate(cardPrefab, new Vector2(0,0), Quaternion.identity);
            newCard.transform.SetParent(gameObject.transform);
            newCard.GetComponent<CardBehavior>().RenderCard(card);
        }
    }
    public override void PickReward()
    {
        base.PickReward();
       
        Debug.Log("I chose a multi card reward");
    }
}
