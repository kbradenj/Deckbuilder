using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardsManager : MonoBehaviour
{
    //Game Objects
    public GameObject cardRewardPrefab;
    public GameObject multiCardRewardPrefab;
    public GameObject playerStatPrefab;
    public GameObject rewardOptionPrefab;
    public GameObject rewardArea;
    public GameObject rewardOptionsArea;
    public GameObject selectedOption;

    //Singleton
    private Singleton singleton;
    
    //States
    public bool optionsShowing = false;

    //Temp Ints
    public int numOfRewards = 3;

    //Lists
    public List<string> rewardType = new List<string>();
    public List<GameObject> rewardObjects;
    public List<Card> shownCards;
    public List<StatReward> shownStat;

    void Start()
    {
        rewardOptionsArea = GameObject.Find("Reward Options List");
        singleton = GameObject.FindObjectOfType<Singleton>();
        CreateRewardOptions();
    }

    void CreateRewardOptions()
    {
        rewardType.Add("card");
        rewardType.Add("multiCard");
        rewardType.Add("playerStat");
        for(int i = 0; i < rewardType.Count; i++){
            GameObject rewardOption;
            rewardOption = GameObject.Instantiate(rewardOptionPrefab, new Vector2(0,0), Quaternion.identity);
            rewardOption.transform.SetParent(rewardOptionsArea.transform);
            RewardOption rewardOptionScript = rewardOption.AddComponent<RewardOption>();
            rewardOptionScript.rewardType = rewardType[i];
            rewardOptionScript.RenderOption(rewardType[i]);
        }
        ShowRewardOptions();
    }


    public void SelectReward(GameObject optionObject){
        rewardObjects = new List<GameObject>();
        shownCards = new List<Card>();
        selectedOption = optionObject;
        for(int i = 0; i < 3; i++){
            GameObject prefab;
            RewardOption optionScript = optionObject.GetComponent<RewardOption>();
            int rarity = GetRarity();
            switch(optionScript.rewardType)
            {
                case "card":
                prefab = cardRewardPrefab;
                break;
                case "multiCard":
                prefab = multiCardRewardPrefab;
                break;
                case "playerStat":
                prefab = playerStatPrefab;
                break;
                default:
                prefab = cardRewardPrefab;
                break;
            }
                GameObject reward;
                reward = GameObject.Instantiate(prefab, new Vector2(0,0), Quaternion.identity);
                rewardArea = GameObject.Find("Reward Area");
                reward.transform.SetParent(rewardArea.transform);
                rewardObjects.Add(reward);
                reward.GetComponent<Reward>().rewardsManager = this;
            }
            rewardOptionsArea = GameObject.Find("Reward Options List");
            shownCards.Clear();
            HideRewardOptions();
        
    }

    

     public int GetRarity()
    {
        return Random.Range(1, 3);
    }
    
    public void RemoveRewards()
    {
        int rewardsListCount = rewardObjects.Count;
        for(int i = 0; i < rewardsListCount; i++)
        {
            Destroy(rewardObjects[i].gameObject);
        }
    }

    public void HideRewardOptions()
    {
        rewardOptionsArea.SetActive(false);
        optionsShowing = false;
    }

    public void ShowRewardOptions()
    {
        rewardOptionsArea.SetActive(true);
        optionsShowing = true;
    }
}
