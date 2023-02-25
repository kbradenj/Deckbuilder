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
    public TMP_Text enemyMaxHealthField;
    public TMP_Text enemyActionField;
    public TMP_Text enemyActionTitle;
    public TMP_Text enemyBlockField;
   


    public Image image;
    public Color highlightColor;
    public Color defaultColor;
    public Slider healthSlider;

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
        health = enemy.health;
        maxHealth = enemy.maxHealth;
        healthSlider.value = ((float)health/enemy.maxHealth) * 100;
        level = enemy.level;
        image.sprite = enemy.enemyImage;
        defaultColor = image.color;
        UpdateStats();
    }

    public override void Death()
    {
        gameState.numOfEnemies--;
        Destroy(this.gameObject);
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
        enemyMaxHealthField.text = "/ " + maxHealth.ToString();
        healthSlider.value = ((float)health/enemy.maxHealth) * 100;
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
                case "strength":
                icon.statusTextContainer.text = strength.ToString();
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
            else if(vulnerable == 1)
            {
                vulnerable --;
                RemoveStatusIcon("vulnerable");
            }

            if(weak > 1)
            {
                weak --;
            }
            else if(weak == 1)
            {
                weak --;
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
        AdjustStatus();
        EnemyAction currentAction = enemy.actionList[turnNumber-1];
        switch(currentAction.type)
        {
            case "attack":
            int attackAmount = (int)Math.Ceiling(currentAction.baseAmount * weaknessMod) + strength;
            actions.Attack(player, attackAmount, currentAction.multiAction);
            break;
            case "block":
            actions.Block(this, currentAction.baseAmount);
            break;
            case "vulnerable":
            break;
            case "strength":
            actions.Strength(this, currentAction.baseAmount);
            break;
        }

        if(turnNumber == enemy.actionList.Count){
            turnNumber = 1;
        }
        else{
            turnNumber++;
        }

        player.block = 0; //maybe move to start turn?
        
        NextTurn();
        
      
        
    }

    public override void NextTurn(){
        EnemyAction nextAction = enemy.actionList[turnNumber-1];
        enemyActionTitle.text = nextAction.type;
        int modDamage = nextAction.baseAmount;
        if(nextAction.type == "attack")
        {
            modDamage = (int)Math.Ceiling(nextAction.baseAmount * weaknessMod) + strength;
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


