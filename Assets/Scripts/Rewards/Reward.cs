using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
    public Singleton singleton;
    public RewardsManager rewardsManager;
    public string rewardType;
    public int rarity;

    protected virtual void Awake()
    {
        singleton = GameObject.FindObjectOfType<Singleton>();
        // rarity = rewardsManager.GetRarity();
    }

    public virtual void PickReward(){
        rewardsManager.RemoveRewards();
        rewardsManager.ShowRewardOptions();
        Destroy(rewardsManager.selectedOption);
    }

    public virtual void Render()
    {
        
    }
}
