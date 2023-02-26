using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardsManager : MonoBehaviour
{
    public int numOfRewards = 3;
    public GameObject cardRewardPrefab;
    public GameObject multiCardRewardPrefab;
    public GameObject maxHealthRewardPrefab;
    public GameObject rewardArea;
    Singleton singleton;
    
    void Start()
    {
        rewardArea = GameObject.Find("Reward Area");
        singleton = GameObject.FindObjectOfType<Singleton>();
        SelectReward();
    }

    void SelectReward(){
            GameObject reward = GameObject.Instantiate(cardRewardPrefab, new Vector2(0,0), Quaternion.identity);
            reward.GetComponent<CardReward>().card = singleton.cardDatabase[5];
            reward.transform.SetParent(rewardArea.transform);
            reward = GameObject.Instantiate(multiCardRewardPrefab, new Vector2(0,0), Quaternion.identity);
            List <Card> multiRewardsList = reward.GetComponent<MultiCardReward>().multiRewards;
            multiRewardsList.Add(singleton.cardDatabase[0]);
            multiRewardsList.Add(singleton.cardDatabase[0]);
            multiRewardsList.Add(singleton.cardDatabase[0]);
            reward.transform.SetParent(rewardArea.transform);
            reward = GameObject.Instantiate(maxHealthRewardPrefab, new Vector2(0,0), Quaternion.identity);
            reward.transform.SetParent(rewardArea.transform);
    }
}
