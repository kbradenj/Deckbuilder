using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scavenge : MonoBehaviour
{
    public TMP_Text totalPriceText;

    public GameObject cardPrefab;
    private GameObject scavengeCardArea;

    private Singleton singleton;

    private List<GameObject> scavengeCardObjects;

    public int storeLevel;
    public int cartTotal;

    void Start()
    {
        
        scavengeCardObjects = new List<GameObject>();
        singleton = FindObjectOfType<Singleton>();
        if(singleton.storeLevel != 0){
            storeLevel = singleton.storeLevel;
        }
        else
        {
            singleton.storeLevel = 1;
        }
        singleton.AdjustDaylight();
        scavengeCardArea = GameObject.Find("Scavenge Card Area");
        LoadStore();
    }

    public void LoadStore()
    {
        for(int i = 1; i <= singleton.cardDictionary[storeLevel].Count; i++) //4
        {
            if(singleton.cardDictionary[storeLevel][i].Count != 0){
                for(int j = 0; j < singleton.cardDictionary[storeLevel][i].Count; j++)
                {
                    GameObject scavengeCard = GameObject.Instantiate(cardPrefab, Vector2.zero, Quaternion.identity);
                    ScavengeCard scavengeScript = scavengeCard.AddComponent<ScavengeCard>();
                    scavengeScript.scavengeScript = this;
                    scavengeCardObjects.Add(scavengeCard);
                    CardBehavior cardScript = scavengeCard.GetComponent<CardBehavior>();
                    cardScript.RenderCard(singleton.cardDictionary[storeLevel][i][j], true);
                    cardScript.card.quantity = 0;
                    cardScript.UpdateQuantity(0);
                    scavengeCard.transform.SetParent(scavengeCardArea.transform);
                    cardScript.AddPrice(scavengeCard, cardScript.card.price);
                    cardScript.UpdatePriceText("Price: " + cardScript.card.price);
              
                }
            }
        }
    }

    public void UpgradeStore()
    {
        singleton.storeLevel++;
        storeLevel++;
        RemoveCards();
        LoadStore();
    }

    public void RemoveCards()
    {
        foreach(GameObject card in scavengeCardObjects)
        {
            Destroy(card);
        }
    }

    public void SelectScavengeCard(GameObject scavengeCard)
    {
        CardBehavior cardBehavior = scavengeCard.gameObject.GetComponentInChildren<CardBehavior>();
        if(singleton.dayLeft - (cartTotal + cardBehavior.card.price) > 0)
        {
            cardBehavior.card.quantity++;
            int newQuantity = cardBehavior.card.quantity;
            cardBehavior.UpdateQuantity(newQuantity);
            AddToCart(cardBehavior.card.price);
        }
        else{
            Debug.Log("Not Enough Daylight");
        }

    }

    public void AddToCart(int price)
    {
        cartTotal += price;
        totalPriceText.text = "Total Price: " + cartTotal;
    }
}
