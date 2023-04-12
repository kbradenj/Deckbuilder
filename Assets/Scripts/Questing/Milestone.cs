using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Milestone : MonoBehaviour
{

    private GameObject milestoneRewardList;
    [SerializeField]GameObject milestoneRewardPrefab;

    private void Start() {
        milestoneRewardList = GameObject.Find("PossibleRewardsList");
        LoadRewards();
    }

    void LoadRewards()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject milestoneReward = GameObject.Instantiate(milestoneRewardPrefab, Vector2.zero, Quaternion.identity);
            milestoneReward.transform.SetParent(milestoneRewardList.transform);
        }
    }
}
