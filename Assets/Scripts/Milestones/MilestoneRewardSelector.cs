using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MilestoneRewardSelector : MonoBehaviour
{
    TMP_Text headline;
    TMP_Text instructions;
    [SerializeField]GameObject milestoneRewardPrefab;
    GameObject rewardArea;

    public MilestoneReward selectedReward;
    MilestoneManager milestoneManager;

    void Start()
    {
        headline = GameObject.Find("Headline").GetComponent<TMP_Text>();
        instructions = GameObject.Find("Instructions").GetComponent<TMP_Text>();
        rewardArea = GameObject.Find("RewardArea");
        milestoneManager = FindObjectOfType<MilestoneManager>();
        MilestoneSelectionSceneSetup();
    }

    public void MilestoneSelectionSceneSetup()
    {
        headline.text = "You have reached Day 6!";
        milestoneManager.GetRewards(6);
        foreach(Milestone milestone in milestoneManager.GetRewards(6))
        {
            GameObject milestoneRewardObject = GameObject.Instantiate(milestoneRewardPrefab, Vector2.zero, Quaternion.identity);
            milestoneRewardObject.transform.SetParent(rewardArea.transform);
            milestoneRewardObject.GetComponent<MilestoneReward>().milestone = milestone;
            if(milestone.type == "card")
            {
                
            }
        }
    }

    public void ConfirmReward()
    {
        milestoneManager.EnableReward(selectedReward.milestone);
        FindAnyObjectByType<Navigation>().Navigate("Home");
    }
   
}
