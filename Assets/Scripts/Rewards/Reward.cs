using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    public Singleton singleton;
    public RewardsManager rewardsManager;
    public string rewardType;
    public int rarity;
    public int cardCount;
    public int randomRarity;
    public Color defaultBGColor;
    public Color highlightColor;
    public Card chosenReward;



    protected virtual void Awake()
    {
        singleton = GameObject.FindObjectOfType<Singleton>();
        rewardsManager = FindObjectOfType<RewardsManager>();
    }

    public virtual void ConfirmReward(){
        rewardsManager.ShowRewardOptions();
    }

    public virtual void SelectReward()
    {
        if(rewardsManager.selectedReward != null)
        {
            rewardsManager.selectedReward.StopHighlight();
        }
     
        rewardsManager.confirmButton.interactable = true;
        rewardsManager.selectedReward = this;
        Highlight();
   
    }

    public virtual void Highlight()
    {
        
    }

    public virtual void StopHighlight()
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
            if (!IsDuplicate(chosenReward) && !chosenReward.isLocked)
            {
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
            }
        }
    }
    return chosenReward;
}

    public bool IsDuplicate(Card reward)
    {   
        bool matched = false;
        foreach(Card card in rewardsManager.shownCards)
        {
            if(reward.cardName == card.cardName)
            {
                matched = true;
                break;
            }
        }
        return matched;
    }
}
