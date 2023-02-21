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
    public TMP_Text statusText;

    //Game Objects
    public GameObject statusIconsArea;
    public GameObject statusIcon;
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
        health = enemy.health;
        level = enemy.level;
        image.sprite = enemy.enemyImage;
        defaultColor = image.color;
        UpdateStats();
    }

    public override void AddStatusIcon(string icon)
    {
        
        statusIcon = Instantiate(statusIconPrefab, new Vector2 (0, 0), Quaternion.identity);
        statusIcon.transform.SetParent(statusIconsArea.transform, false);

        statusImage = statusIcon.GetComponent<Image>();
        statusImage.sprite = Resources.Load<Sprite>("StatusIcons/" + icon);

        statusText = statusIcon.GetComponentInChildren<TMP_Text>();
        statusText.text = vulnerable.ToString();
    }

    public override void RemoveStatusIcon(string icon)
    {
        Destroy(statusIcon);
    }

    public override void UpdateStats()
    {
        enemyHealthField.text = health.ToString();
        enemyBlockField.text = block.ToString();
    }

    public override void UpdateStatus()
    {
        statusText.text = vulnerable.ToString();
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
        
        if(vulnerable -1 > 0)
        {
            vulnerable -= 1;
             UpdateStatus();
        }
        else
        {
            RemoveStatusIcon("vulnerable");
        }
       
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
