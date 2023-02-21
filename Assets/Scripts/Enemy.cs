using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Enemy : Character
{
    //TMP
    public TMP_Text enemyHealthField;
    public TMP_Text enemyActionField;
    public TMP_Text enemyActionTitle;
    public TMP_Text enemyBlockField;
    public TMP_Text statusText;
    public List<StatusIcon> statusIcons;

    //Game Objects
    public GameObject statusIconsArea;
    public GameObject statusIcon;
    public GameObject[] statuses;
    public Image statusImage;

    public Image image;
    public Color highlightColor;
    public Color defaultColor;

    //Scripts
    private Player player;
    public EnemyObject[] database;
    public EnemyObject enemy;
    public Actions actions;

    //Enemy Stats
    public int attack;

    //Battle Counters
    public int turnNumber = 1;

    void Start()
    {
        player = FindObjectOfType<Player>();
        actions = FindObjectOfType<Actions>();
        statusIcons = new List<StatusIcon>();
        health = enemy.health;
        level = enemy.level;
        image.sprite = enemy.enemyImage;
        defaultColor = image.color;
        UpdateStats();
    }

    public override void AddStatusIcon(string icon, int amount)
    {
        
        statusIcon = Instantiate(statusIconPrefab, new Vector2 (0, 0), Quaternion.identity);
        statusIcon.transform.SetParent(statusIconsArea.transform, false);

        statusImage = statusIcon.GetComponent<Image>();
        statusImage.sprite = Resources.Load<Sprite>("StatusIcons/" + icon);

        statusText = statusIcon.GetComponentInChildren<TMP_Text>();
        statusText.text = amount.ToString();

        StatusIcon statusIconItem = new StatusIcon {type = icon, statusAmount = amount, statusIconContainer = statusIcon, statusTextContainer = statusText};
        statusIcons.Insert(0, statusIconItem);
    }

    public override void RemoveStatusIcon(string icon)
    {
        foreach(StatusIcon status in statusIcons)
        {
            if(status.type == icon){
                Destroy(status.statusIconContainer);
            }
        }
    }

    public override void UpdateStats()
    {
        enemyHealthField.text = health.ToString();
        enemyBlockField.text = block.ToString();
    }

    public override void UpdateStatus()
    {
        foreach (StatusIcon icon in statusIcons){
            switch(icon.type)
            {
                case "weak":
                icon.statusTextContainer.text = weak.ToString();
                break;
                case "vulnerable":
                icon.statusTextContainer.text = vulnerable.ToString();
                break;
            }
        }
    }

    public override void AdjustStatus()
    {
        if(statusIcons == null){
            return;
        } 
        else
        {
            if(vulnerable > 1)
            {
                vulnerable --;
            }
            else
            {
                RemoveStatusIcon("vulnerable");
            }

            if(weak > 1)
            {
                weak --;
            }
            else
            {
                RemoveStatusIcon("weak");
                weaknessMod = 1;
            }
                UpdateStatus();
        }
    }

    public void EnemyTurn()
    {
        block = 0;
        UpdateStats();

        EnemyAction currentAction = enemy.actionList[turnNumber-1];
        switch(currentAction.type)
        {
            case "attack":
            actions.Attack(player, currentAction.baseAmount, currentAction.multiAction);
            break;
            case "block":
            actions.Block(this, currentAction.baseAmount);
            break;
            case "vulnerable":
            break;
        }

        if(turnNumber == enemy.actionList.Count){
            turnNumber = 1;
        }
        else{
            turnNumber++;
        }

        player.block = 0; //maybe move to start turn?
        AdjustStatus();
        NextTurn();
      
        
    }

    public override void NextTurn(){
        EnemyAction nextAction = enemy.actionList[turnNumber-1];
        enemyActionTitle.text = nextAction.type;
        int modDamage = nextAction.baseAmount;
        if(nextAction.type == "attack")
        {
            modDamage = (int)Math.Ceiling(nextAction.baseAmount * weaknessMod);
        }
        if(nextAction.multiAction > 1){
            enemyActionField.text = modDamage + " * " + nextAction.multiAction;
        }
        else{
            enemyActionField.text = modDamage.ToString();
        }
        UpdateStats();
    }

    public void Highlight()
    {
        image.color = highlightColor;
    }

    public void StopHighlight()
    {
        image.color = defaultColor;
    }
}

public class StatusIcon 
{
    public string type {get; set;}
    public int statusAmount {get; set;}
    public GameObject statusIconContainer {get; set;}
    public TMP_Text statusTextContainer {get; set;}
}
