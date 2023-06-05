using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerStatReward : Reward
{
    public List<StatReward> statRewardOptions = new List<StatReward>();
    public StatReward[] statRewardDatabase;
    public GameObject playerStatPrefab;
    private Player player;

    public TMP_Text statText;
    public Image image;
    public StatReward chosenStatReward;
    private string statType;
    private int amount;

    protected override void Awake()
    {
        base.Awake();
        statRewardDatabase = Resources.LoadAll<StatReward>("Stat Rewards");
        player = GameObject.FindObjectOfType<Singleton>().player;
        singleton = GameObject.FindObjectOfType<Singleton>();

        foreach(StatReward reward in statRewardDatabase)
        {
            statRewardOptions.Add(reward);
        }
        RandomStatSelection();
        DisplayStatRewardInfo();
    }

    public override void ConfirmReward()
    {
        BaseStatIncrease();
        base.ConfirmReward();
    }

    public void DisplayStatRewardInfo()
    {
        statText.text = "Increase your " + chosenStatReward.statType + " by " + (int)Mathf.Ceil(chosenStatReward.baseAmount * player.level * chosenStatReward.modifier);
        image.sprite = chosenStatReward.sprite;
    }

    public void RandomStatSelection()
    {
        bool statFound = false;
        int counter = 0;
        while((statFound == false) || counter > 500)
        {
            int rndNum = Random.Range(1, statRewardOptions.Count);
            StatReward stat = statRewardOptions[rndNum];
            if(!IsDuplicate(stat))
            {
                chosenStatReward = stat;
                amount = (int)Mathf.Ceil(chosenStatReward.baseAmount * player.level * chosenStatReward.modifier);
                rewardsManager.shownStat.Add(chosenStatReward);
                statFound = true;
            }
            else
            {
                statFound = false;
            }
            counter++;
        }
    }

    public bool IsDuplicate(StatReward stat)
    {
        bool matched = false;
        foreach(StatReward statReward in rewardsManager.shownStat)
        {
            if(statReward.statType == stat.statType)
            {
                matched = true;
                break;
            }
        }
        return matched;
    }

    public void BaseStatIncrease()
    {   
        string statType = chosenStatReward.statType;

        switch(statType)
        {
            case "strength":
            player.baseStrength += amount;
            break;
            case "dexterity":
            player.baseDefense += amount;
            break;
            case "maxHealth":
            player.maxHealth += amount;
            singleton.HealPlayer(amount);
            break;
            case "health":
            singleton.HealPlayer(amount);
            break;
        }
    }

    public override void Highlight()
    {
        base.Highlight();
        defaultBGColor = image.color;
        image.color = highlightColor;

    }

    public override void StopHighlight()
    {
        base.StopHighlight();
        image.color = defaultBGColor;
    }
}

