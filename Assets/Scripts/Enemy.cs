using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Enemy : Character
{
    //TMP
    public TMP_Text enemyHealthField;
    public TMP_Text enemyActionField;
    public TMP_Text enemyActionTitle;
    public TMP_Text enemyBlockField;
    public GameObject statusField;

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
        health = enemy.health;
        level = enemy.level;
        image.sprite = enemy.enemyImage;
        defaultColor = image.color;
        UpdateStats();
        
    }

    public override void AddStatusIcon(string icon)
    {
        Sprite statusIcon = Resources.Load<Sprite>("StatusIcons/" + "icon");
        GameObject vulnerableIcon = Instantiate(statusIconPrefab, new Vector2 (0, 0), Quaternion.identity);
        vulnerableIcon.transform.SetParent(statusField.transform, false);
    }

    public override void UpdateStats()
    {
        enemyHealthField.text = health.ToString();
        enemyBlockField.text = block.ToString();
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

        EnemyNextTurn(enemy.actionList[turnNumber-1]);

    }

    public void EnemyNextTurn(EnemyAction nextAction){
        enemyActionTitle.text = "Next: " + nextAction.type;
        if(nextAction.multiAction > 1){
            enemyActionField.text = nextAction.baseAmount + " * " + nextAction.multiAction;
        }
        else{
            enemyActionField.text = nextAction.baseAmount.ToString();
        }
        

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
