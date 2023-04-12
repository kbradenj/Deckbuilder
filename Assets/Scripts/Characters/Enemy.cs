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

    public GameObject highlightPS;

    //Scripts
    private Battle battle;
    private Player player;
    public EnemyObject[] database;
    public EnemyObject enemy;
    private ActionManager actionManager;

    //Enemy Stats
    public int attack;

    //Battle Counters
    public int turnNumber = 1;

    void Start()
    {  
        player = singleton.player;
        battle = FindObjectOfType<Battle>();
        actionManager = FindObjectOfType<ActionManager>();
        health = enemy.health;
        maxHealth = enemy.maxHealth;
        healthSlider.value = ((float)health/enemy.maxHealth) * 100;
        image.sprite = enemy.enemyImage;
        defaultColor = image.color;
        UpdateStats();
    }

    public override void Death()
    {
        battle.numOfEnemies--;
        Destroy(this.gameObject);
    }

    public override void UpdateStats()
    {
        enemyHealthField.text = health.ToString();
        enemyMaxHealthField.text = "/ " + maxHealth.ToString();
        healthSlider.value = ((float)health/enemy.maxHealth) * 100;
        enemyBlockField.text = block.ToString();
    }

    public void EnemyTurn()
    {
        StartTurn();
        UpdateStats();
        EnemyAction currentAction = enemy.actionList[turnNumber-1];
        switch(currentAction.type)
        {
            case "attack":
            int attackAmount = (int)Math.Ceiling(currentAction.baseAmount * weaknessMod) + strength;
            actionManager.Attack(player, attackAmount, currentAction.multiAction);
            break;
            case "block":
            actions.Block(this, currentAction.baseAmount);
            break;
            case "vulnerable":
            break;
            case "strength":
            actions.AddEffect(player, currentAction.baseAmount, ref strength, "strength");
            break;
            case "poison":
            actions.AddEffect(player, currentAction.baseAmount, ref player.poison, "poison");
            break;
            case "shriek":
            actions.AddEffect(player, currentAction.baseAmount, ref player.shriek, "shriek");
            break;
            case "fear":
            actions.AddEffect(player, currentAction.baseAmount, ref player.fear, "fear");
            break;
            case "weak":
            actions.AddEffect(player, currentAction.baseAmount, ref player.weak, "weak");
            break;
        }

        if(turnNumber == enemy.actionList.Count){
            turnNumber = 1;
        }
        else{
            turnNumber++;
        }
        
        NextTurn();
        EndTurn();
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
        highlightPS.gameObject.SetActive(true);
    }

    public void StopHighlight()
    {
        image.color = defaultColor;
        highlightPS.gameObject.SetActive(false);
    }
}


