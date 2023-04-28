using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MilestoneManager : MonoBehaviour
{

    private Singleton singleton;
    private GameObject milestoneRewardList;
    [SerializeField]GameObject milestoneLogPrefab;
    [SerializeField]GameObject milestonePrefab;
    [SerializeField]GameObject milestoneRewardPrefab;

    private Milestone[] milestoneDatabase;
    public Dictionary<int, List<Milestone>> milestoneCatalog = new Dictionary<int, List<Milestone>>();

    void Start()
    {

        singleton = FindObjectOfType<Singleton>();
        milestoneDatabase = Resources.LoadAll<Milestone>("Milestones");
 
        if(singleton.milestoneCatalog.Count <= 0)
        {
            CreateMilestoneCatalog();
        }
    }

    public void CreateMilestoneCatalog()
    {
        for(int i = 6; i < 25; i += 5)
        {
            List<Milestone> tempList = new List<Milestone>();
            foreach(Milestone milestone in milestoneDatabase)
            {
                if(milestone.day == i)
                {
                    tempList.Add(milestone);
                }
            }
            milestoneCatalog.Add(i, tempList);
        }
        singleton.milestoneCatalog = milestoneCatalog;
    }

    public void OpenMilestoneBook()
    {
        GameObject milestoneLog = GameObject.Instantiate(milestoneLogPrefab, new Vector2(Screen.width/2f, Screen.height/2f), Quaternion.identity);
        milestoneLog.transform.SetParent(GameObject.Find("Main Canvas").transform);
    }

    public void EnableReward(Milestone milestone)
    {
        switch(milestone.type)
        {
            case "recipe":
                singleton.recipeDictionary[milestone.identifier].isLocked = false;
            break;
            case "feature":
                switch(milestone.identifier)
                {
                    case "heal":
                    singleton.unlocks.isHealLocked = false;
                    break;
                    case "scavenge":
                    singleton.unlocks.isScavengeLocked = false;
                    break;
                }
            break;
            case "card":
                singleton.cardLookup[milestone.identifier].isLocked = false;
            break;
        }
        milestone.isLocked = false;
    }

    public List<Milestone> GetRewards(int milestoneDay = 0)
    {
        List<Milestone> availableMilestoneRewards = new List<Milestone>();
        if(singleton.milestoneCatalog.ContainsKey(milestoneDay))
        {
            foreach(Milestone milestone in singleton.milestoneCatalog[milestoneDay])
            {
                if(milestone.isLocked)
                {
                    availableMilestoneRewards.Add(milestone);
                }
            }
        }
        return availableMilestoneRewards;
    }


}
