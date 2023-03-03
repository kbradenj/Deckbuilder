using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameState : MonoBehaviour
{
    //Game Objects
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject playerArea;
    public GameObject enemyArea;

    //Singleton
    public Singleton singleton;

    //Scripts
    public Player player;
    public List<Enemy> enemies;
    public CardManager cardManager;
    public EnemyAction[] enemyActionDatabase;
    
    //TMPro
    public TMP_Text homeDaylightCount;

    //Bools
    public bool isBattle = false;
    private bool enemiesLoaded = false;

    //Resources
    public EnemyObject[] enemyDatabase;
    public List<Card> playerDeck;
    public Card[] database;
    public List<Card> cardDatabase = new List<Card>();

    //Counters
    public int numOfEnemies = 0;

    void Awake()
    {
        //Get singleton, check to see if the card db is loaded
        singleton = GameObject.FindObjectOfType<Singleton>();
        if(singleton.cardDatabase.Count <= 0){
            LoadCardDatabase();
        }
        
       
        //Update UI showing daylight at camp
        if(SceneManager.GetActiveScene().name == "Home")
        {
             UpdateHomeDaylightCount();
        }

        //Set up Card Manager
        cardManager = FindObjectOfType<CardManager>();
        if(singleton.playerDeck.Count <= 0)
        {
            cardManager.CreatePlayerDeck(); 
        }

        //If this scene is a battle, let's battle
        if(SceneManager.GetActiveScene().name == "Battle")
        {
            playerArea = GameObject.Find("Player Area");
            enemyArea = GameObject.Find("Enemy Area");
            isBattle = true;
            LoadEnemies();
            StartBattle();
        }

    }

    void Update()
    {
        //Is the battle over?
        if(numOfEnemies == 0 && enemiesLoaded && isBattle){
            singleton.player.health = player.health;
            isBattle = false;
            SceneManager.LoadScene("WinScreen");
            return;
        }
    }

    //Update UI to show current daylight
    public void UpdateHomeDaylightCount(){
        homeDaylightCount.text = singleton.dayLeft.ToString() + " minutes left in the day";
    }

    //Pull in card db
    public void LoadCardDatabase()
    {
        database = Resources.LoadAll<Card>("Cards");
        for(int i = 0; i < database.Length; i++)
        {
            cardDatabase.Add(database[i]);
        }
        singleton.cardDatabase = cardDatabase;
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
        singleton.player = player;
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
        player = GameObject.FindObjectOfType<Player>();
        player.UpdateStats();
        cardManager.LoadPlayerDeck(player);
    }

    //End Turn Button
    public void EndTurn()
    {
        cardManager.Discard();
        cardManager.Draw(player.drawSize);
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




