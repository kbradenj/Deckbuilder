using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Scavenge : MonoBehaviour
{

    public GameState gameState;
    public TMP_Text totalPriceText;

    public GameObject cardPrefab;
    public GameObject artifactPrefab;
    private GameObject scavengeCardArea;
    private GameObject artifactArea;
    private Singleton singleton;

    private List<GameObject> scavengeCardObjects = new List<GameObject>();
    private List<Card> cart = new List<Card>();

    public int storeLevel;
    public int cartTotal;

    void Start()
    {
        singleton = FindObjectOfType<Singleton>();
        gameState = FindObjectOfType<GameState>();
        scavengeCardArea = GameObject.Find("Scavenge Card Area");
        artifactArea = GameObject.Find("Artifact Area");

        if(singleton.storeLevel != 0){
            storeLevel = singleton.storeLevel;
        }
        else
        {
            singleton.storeLevel = 1;
        }
        singleton.AdjustDaylight();
        LoadArtifacts();
        
    }

    public void LoadStore()
    {
        scavengeCardObjects.Clear();
        for(int i = 1; i <= storeLevel; i++)
        {
            for(int j = 1; j <= singleton.cardDictionary[i].Count; j++)
            {
                if(singleton.cardDictionary[storeLevel][i].Count != 0){
                    foreach(KeyValuePair<int, Card> kvp in singleton.cardDictionary[i][j])
                    {
                        GameObject scavengeCard = GameObject.Instantiate(cardPrefab, Vector2.zero, Quaternion.identity);
                        ScavengeCard scavengeScript = scavengeCard.AddComponent<ScavengeCard>();
                        scavengeScript.scavengeScript = this;
                        scavengeCardObjects.Add(scavengeCard);
                        CardBehavior cardScript = scavengeCard.GetComponent<CardBehavior>();
                        cardScript.RenderCard(kvp.Value, true);
                        cardScript.card.quantity = 0;
                        cardScript.UpdateQuantity(0);
                        scavengeCard.transform.SetParent(scavengeCardArea.transform);
                        cardScript.AddPrice(scavengeCard, cardScript.card.price);
                        cardScript.UpdatePriceText("Price: " + cardScript.card.price);
                    }
                }
            }
        }
    }

    public void LoadArtifacts()
    {
        var rand = new System.Random();
        Artifact[] randomArtifacts = gameState.artifactDatabase.OrderBy(_ => rand.Next()).Take(2).ToArray();
        foreach(Artifact artifact in randomArtifacts)
        {
            GameObject artifactObject = GameObject.Instantiate(artifactPrefab, Vector2.zero, Quaternion.identity);
            DisplayArtifact displayArtifact = artifactObject.GetComponent<DisplayArtifact>();
            displayArtifact.thisArtifact = artifact;
            displayArtifact.RenderArtifact();
            artifactObject.transform.SetParent(artifactArea.transform);

            
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
            AddToCart(cardBehavior.card, cardBehavior.card.price);
        }
        else{
            Debug.Log("Not Enough Daylight");
        }

    }

    public void AddToCart(Card card, int price)
    {
        cart.Add(card);
        cartTotal += price;
        totalPriceText.text = "Total Price: " + cartTotal;
    }

    public void Checkout()
    {
        int initialCartCount = cart.Count;
        for(int i = 0; i < initialCartCount; i++)
        {
            singleton.playerDeck.Add(cart[0]);
        }
        cart.Clear();
        Debug.Log("Scavenge Card Count " + scavengeCardObjects.Count);
        foreach(GameObject cardObject in scavengeCardObjects)
        {
            CardBehavior cardScript = cardObject.GetComponent<CardBehavior>();
            cardScript.UpdateQuantity(0);
            cardScript.card.quantity = 0;
        }
        singleton.AdjustDaylight(cartTotal);
        cartTotal = 0;
    }
}
