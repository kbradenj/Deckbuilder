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
   
    //UI
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
    public int baseAttack;
    public float dayMod = .25f;
    public float levelMod = 1;

    //Battle Counters
    private int actionIndex = 0;

    void Start()
    {  
        levelMod *= singleton.dayCount > 1 ? 1 + (singleton.dayCount * dayMod) : 1;
        player = singleton.player;
        battle = FindObjectOfType<Battle>();
        actionManager = FindObjectOfType<ActionManager>();
        maxHealth = (int)Math.Round(enemy.maxHealth * levelMod);
        health = maxHealth;
        defense = (int)Math.Round(enemy.baseDefense * levelMod);
        baseAttack = (int)Math.Round(enemy.baseAttack * levelMod);
        healthSlider.value = (health/maxHealth) * 100;
    
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
        block = 0;
        UpdateStats();
   
        EnemyAction currentAction = enemy.actionList[actionIndex-1];
        switch(currentAction.type)
        {
            case "attack":
            int attackAmount = (int)Math.Ceiling(baseAttack * weaknessMod) + strength;
            actionManager.Attack(player, attackAmount, currentAction.multiAction);
            break;
            case "block":
            actions.Block(this, enemy.baseDefense);
            break;
            case "vulnerable":
            actions.AddEffect(player, currentAction.baseAmount, ref player.vulnerable, "vulnerable");
            break;
            case "strength":
            actions.AddEffect(this, currentAction.baseAmount, ref strength, "strength");
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

        if(actionIndex == enemy.actionList.Count){
            actionIndex = 1;
        }
        else{
            actionIndex++;
        }
        
        NextTurn();
        EndTurn();
    }

    public override void NextTurn(){
        if(actionIndex == 0)
        {
            if(enemy.setAttackOrder)
            {
                actionIndex = 1;
            }
            else
            {
                 int actionListCount = enemy.actionList.Count;
                actionIndex = UnityEngine.Random.Range(1, actionListCount);
            }
           
        }
        EnemyAction nextAction = enemy.actionList[actionIndex-1];
        enemyActionTitle.text = nextAction.type;
        int actionAmount = nextAction.baseAmount;

        actionAmount = nextAction.type == "attack" ? (int)Math.Ceiling(enemy.baseAttack * weaknessMod) + strength : actionAmount;

        actionAmount = nextAction.type == "block" ? enemy.baseDefense : actionAmount;
        if(nextAction.multiAction > 1){
            enemyActionField.text = actionAmount + " * " + nextAction.multiAction;
        }
        else{
            enemyActionField.text = actionAmount.ToString();
        }
        if(nextAction.type == "wait")
        {
            enemyActionField.text = "";
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


