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
        statText.text = "Increase your " + chosenStatReward.statType + " by " + (int)Mathf.Ceil(chosenStatReward.baseAmount * player.level * chosenStatReward.modifier);
        image.sprite = chosenStatReward.sprite;
    }

    public void RandomStatSelection()
    {
        int rndNum = Random.Range(1, statRewardOptions.Count);
        StatReward stat = statRewardOptions[rndNum];
        chosenStatReward = stat; 
        statType = chosenStatReward.statType;
        amount = (int)Mathf.Ceil(chosenStatReward.baseAmount * player.level * chosenStatReward.modifier);
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

