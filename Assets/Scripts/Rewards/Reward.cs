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

    public Card chosenReward;



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

    public virtual Card GetRandomCard(int levelMin, int levelMax, bool allowDuplicates)
    {
    bool cardFound = false;
    Dictionary<int, Dictionary<int, Card>> levelDictionary = singleton.cardDictionary[Random.Range(levelMin, levelMax)];
    cardCount = 0;
    int counter = 0;
    int shownCardsCount = rewardsManager.shownCards.Count;
    while ((cardCount == 0 && cardFound == false) || counter > 500)
    {   
        randomRarity = Random.Range(1, levelDictionary.Count + 1);
        cardCount = levelDictionary[randomRarity].Count;
        counter++;
        if (cardCount > 0)
        {
            int randomCardIndex = Random.Range(0, cardCount);
            chosenReward = levelDictionary[randomRarity][randomCardIndex];
            if (!IsDuplicate(chosenReward))
            {
                Debug.Log("Adding " + chosenReward.cardName + " to the options");
                cardFound = true;
                if (!allowDuplicates)
                {
                    rewardsManager.shownCards.Add(chosenReward);
                }
            }
            else
            {
                cardFound = false;
                cardCount = 0;
                Debug.Log("Duplicate " + chosenReward.cardName + " found, skipping");
            }
        }
    }
    return chosenReward;
}

    public bool IsDuplicate(Card reward)
    {   
        Debug.Log("IsDuplicate ran and passed in " + reward.cardName);
        bool matched = false;
        foreach(Card card in rewardsManager.shownCards)
        {
            Debug.Log(reward.cardName + " vs" + card.cardName);
            if(reward.cardName == card.cardName)
            {
                matched = true;
                break;
            }
        }
        Debug.Log("Matched = " + matched);
        return matched;
    }
}
