using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    //Game Objects
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject playerArea;
    public GameObject enemyArea;

    //Scripts
    public Player player;
    public List<Enemy> enemies;
    public CardManager cardManager;
    public EnemyAction[] enemyActionDatabase;
    
    //Bools
    public bool isBattle = true;
    private bool enemiesLoaded = false;

    //Resources
    public EnemyObject[] enemyDatabase;

    //Counters
    public int numOfEnemies = 0;

    void Start()
    {
        cardManager = FindObjectOfType<CardManager>();
        playerArea = GameObject.Find("Player Area");
        enemyArea = GameObject.Find("Enemy Area");
        cardManager.LoadCardDatabase();
        LoadEnemies();
        StartBattle();
    }

    void Update()
    {
        if(numOfEnemies == 0 && enemiesLoaded){
            SceneManager.LoadScene("WinScreen");
            return;
        }
    }

    //Load Enemies
    public void LoadEnemies()
    {
        enemyDatabase = Resources.LoadAll<EnemyObject>("Enemies");
    }

    //Remove Enemies
    public void RemoveEnemies()
    {

    }

    //Create Player
    public void CreatePlayer()
    {
        GameObject playerObject = GameObject.Instantiate(playerPrefab, new Vector2(0,0), Quaternion.identity) as GameObject;
        playerObject.transform.SetParent(playerArea.transform, false); 
        player = playerObject.GetComponent<Player>();
    }

    //Create Enemy
    public void CreateEnemy()
    {
        for(int i = 0; i < enemyDatabase.Length; i++)
        {
        GameObject enemyNew = GameObject.Instantiate(enemyPrefab, enemyArea.transform.position, Quaternion.identity) as GameObject;
        enemyNew.transform.SetParent(enemyArea.transform);
        Enemy thisEnemy = enemyNew.GetComponent<Enemy>();
        thisEnemy.enemy = enemyDatabase[i];
        thisEnemy.strength = 0;
        thisEnemy.weaknessMod = 1f;
        thisEnemy.NextTurn();
        enemies.Add(thisEnemy);
        numOfEnemies += 1;
        }
        enemiesLoaded = true;
    }

    //Battle States
    public void StartBattle()
    {
        CreatePlayer();
        CreateEnemy();
        player.UpdateStats();
    }

    public void EndTurn()
    {
        cardManager.Discard();
        cardManager.Draw();
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
        
    }

}




