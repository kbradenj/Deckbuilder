using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scavenge : MonoBehaviour
{
    public GameObject cardPrefab;
    private GameObject scavengeCardArea;
    private Singleton singleton;
    private List<GameObject> scavengeCardObjects;
    public int storeLevel;

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
                    scavengeCardObjects.Add(scavengeCard);
                    CardBehavior cardScript = scavengeCard.GetComponent<CardBehavior>();
                    cardScript.displayAmount = false;
                    cardScript.RenderCard(singleton.cardDictionary[storeLevel][i][j]);
                    scavengeCard.transform.SetParent(scavengeCardArea.transform);
                }
            }
        }
    }
    public void UpgradeStore()
    {
        singleton.storeLevel++;
        Debug.Log("Store upgraded and singleton store level is now " + singleton.storeLevel);
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
}
