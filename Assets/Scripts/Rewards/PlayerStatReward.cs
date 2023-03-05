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
    public StatReward chosenReward;
    private string statType;
    private int amount;

    protected override void Awake()
    {
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

    public override void PickReward()
    {
        BaseStatIncrease();
        base.PickReward();
    }

    public void DisplayStatRewardInfo()
    {
        statText.text = "Increase your " + chosenReward.statType + " by " + (int)Mathf.Ceil(chosenReward.baseAmount * player.level * chosenReward.modifier);
        image.sprite = chosenReward.sprite;
    }

    public void RandomStatSelection()
    {
        int rndNum = Random.Range(1, statRewardOptions.Count);
        StatReward stat = statRewardOptions[rndNum];
        chosenReward = stat; 
        statType = chosenReward.statType;
        amount = (int)Mathf.Ceil(chosenReward.baseAmount * player.level * chosenReward.modifier);
    }

    public void BaseStatIncrease()
    {   
        string statType = chosenReward.statType;

        switch(statType)
        {
            case "strength":
            player.baseStrength += amount;
            break;
            case "dexterity":
            player.baseDexterity += amount;
            break;
            case "maxHealth":
            player.maxHealth += amount;
            singleton.HealPlayer(amount);
            break;
            case "health":
            singleton.HealPlayer(amount);
            break;
            default:
            break;
        }
    }
}

