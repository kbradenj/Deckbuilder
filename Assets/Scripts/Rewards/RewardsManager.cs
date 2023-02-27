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
    
    public List<GameObject> rewardType;

    void Start()
    {
        rewardArea = GameObject.Find("Reward Area");
        singleton = GameObject.FindObjectOfType<Singleton>();
        foreach(GameObject reward in rewardType){
            SelectReward(reward);
        }
        
    }

    void SelectReward(GameObject prefab){
            GameObject reward;
            {
            reward = GameObject.Instantiate(prefab, new Vector2(0,0), Quaternion.identity);
            reward.transform.SetParent(rewardArea.transform);
            }
    }
}
