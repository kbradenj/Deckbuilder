using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Battle : MonoBehaviour
{
    private Singleton singleton;
    private GameState gameState;
    private CardManager cardManager;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject playerArea;
    public GameObject enemyArea;

    public List<Enemy> enemies;
    public EnemyObject[] enemyDatabase;
    public EnemyAction[] enemyActionDatabase;

    public Player player;

    private bool enemiesLoaded = false;

    //Counters
    public int numOfEnemies = 0;
    public int enemyAmount = 2;

    public Dictionary<string, Dictionary<string, Card>> powerCards;
    

    void Start()
    {
        singleton = GameObject.FindObjectOfType<Singleton>();
        player = singleton.player;
        gameState = FindObjectOfType<GameState>();
        powerCards = gameState.powerCards;
        
        //Set up Card Manager
        cardManager = FindObjectOfType<CardManager>();
        if(singleton.playerDeck.Count <= 0)
        {
            cardManager.CreatePlayerDeck(); 
        }

        powerCards = new Dictionary<string, Dictionary<string, Card>>();
        playerArea = GameObject.Find("Player Area");
        enemyArea = GameObject.Find("Enemy Area");
        gameState.isBattle = true;
        LoadEnemies();
        StartBattle();
    }

    void Update() {
    
        //Is the battle over?
        if(numOfEnemies == 0 && enemiesLoaded){
            singleton.player.health = player.health;
            gameState.isBattle = false;
            ResetPlayerStats();
            SceneManager.LoadScene("WinScreen");
            return;
        }
    }
    
   //Load Enemies
    public void LoadEnemies()
    {
        enemyDatabase = Resources.LoadAll<EnemyObject>("Enemies");
    }

    //Battle States
    public void StartBattle()
    {
        CreatePlayer();
        CreateEnemy();
        player = GameObject.FindObjectOfType<Player>();
        player.StartTurn();
        player.UpdateStats();
        cardManager.LoadPlayerDeck(player);
    }

      //Create Player
    public void CreatePlayer()
    {
        GameObject playerObject = GameObject.Instantiate(playerPrefab, new Vector2(0,0), Quaternion.identity) as GameObject;
        playerObject.transform.SetParent(playerArea.transform, false); 
        player = singleton.player;
    }

    //Create Enemy
    public void CreateEnemy()
    {
        float enemyWidth = SetEnemyGridWidth();
        for(int i = 0; i < enemyAmount; i++)
        {
            if(enemyDatabase[i].rarity == "Common")
            {
                GameObject enemyNew = GameObject.Instantiate(enemyPrefab, enemyArea.transform.position, Quaternion.identity) as GameObject;
                enemyNew.transform.SetParent(enemyArea.transform);
                Enemy thisEnemy = enemyNew.GetComponent<Enemy>();
                thisEnemy.GetComponent<BoxCollider2D>().size = new Vector2(enemyWidth, 300);
                thisEnemy.enemy = enemyDatabase[i];
                thisEnemy.strength = 0;
                thisEnemy.weaknessMod = 1f;
                thisEnemy.NextTurn();
                enemies.Add(thisEnemy);
                numOfEnemies += 1;
            }
        }
        enemiesLoaded = true;
    }

    //Set card dropzones on enemies
    public float SetEnemyGridWidth()
    {
        GridLayoutGroup gridLayout = enemyArea.GetComponentInChildren<GridLayoutGroup>();
        RectTransform enemyAreaRect = enemyArea.GetComponent<RectTransform>();
        float enemyCellWidth = enemyAreaRect.sizeDelta.x / enemyAmount;
        gridLayout.cellSize = new Vector2 (enemyCellWidth, 300);
        return enemyCellWidth;
    }

    //End Turn Button
    public void EndTurn()
    {
        cardManager.Discard();
        player.EndTurn();
        for(int i = 0; i < enemies.Count; i++){
            if(enemies[i] == null)
            {
                enemies.Remove(enemies[i]);
            }
            else
            {
                enemies[i].EnemyTurn();
            }
        }
        player.StartTurn();
    }

    public void ResetPlayerStats(){
        player.strength = 0;
        player.block = 0;
        player.vulnerable = 0;
        player.weak = 0;
        player.poison = 0;
        singleton.dayLeft = singleton.maxDaylight;
    }
}
