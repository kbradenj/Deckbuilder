using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
    public Singleton singleton;
    public RewardsManager rewardsManager;
    public string rewardType;
    public int rarity;
    public int cardCount;
    public int randomRarity;

    protected virtual void Awake()
    {
        singleton = GameObject.FindObjectOfType<Singleton>();
    }

    public virtual void PickReward(){
        rewardsManager.RemoveRewards();
        rewardsManager.ShowRewardOptions();
        Destroy(rewardsManager.selectedOption);
    }

    public virtual void Render()
    {
        
    }

    public virtual Card GetRandomCard(int levelMin, int levelMax){

        Dictionary<int, Dictionary<int, Card>> levelDictionary = singleton.cardDictionary[Random.Range(levelMin, levelMax)];
        cardCount = 0;
        while(cardCount == 0)
        {
            randomRarity = Random.Range(1, levelDictionary.Count+1);
            cardCount = levelDictionary[randomRarity].Count;
            if(cardCount > 0)
            {
                break;
            }
        }
        
        return levelDictionary[randomRarity][Random.Range(0,cardCount)];
        
        
    }
}
