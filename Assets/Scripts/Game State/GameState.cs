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

    public Dictionary<int, Dictionary<int, Dictionary<int, Card>>> cardDictionary;

    //Bools
    public bool isBattle = false;
    private bool enemiesLoaded = false;

    //Resources
    public EnemyObject[] enemyDatabase;
    public List<Card> playerDeck;
    public Card[] database;
    public List<Card> cardDatabase = new List<Card>();
    public List<Card> ultimateCardDatabase = new List<Card>();

    //Counters
    public int numOfEnemies = 0;

    void Awake()
    {
        //Get singleton, check to see if the card db is loaded
        singleton = GameObject.FindObjectOfType<Singleton>();
        if(singleton.cardDatabase.Count <= 0){
            LoadCardDatabase();
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
            ResetPlayerStats();
            SceneManager.LoadScene("WinScreen");
            return;
        }
    }

    //Pull in card db
    public void LoadCardDatabase()
    {
        database = Resources.LoadAll<Card>("Cards");
          for(int i = 0; i < database.Length; i++)
        {
            cardDatabase.Add(database[i]);
        }
        CreateCardDatabaseLevels();
        singleton.cardDatabase = cardDatabase;
        singleton.cardDictionary = cardDictionary;
    }

    public void CreateCardDatabaseLevels(){
        // Create a dictionary to represent the card levels
        cardDictionary = new Dictionary<int, Dictionary<int, Dictionary<int, Card>>>();

        // Loop through each card level and create a dictionary for each level
        for (int i = 1; i <= 10; i++)
        {
            Dictionary<int, Dictionary<int, Card>> rarityDictionary = new Dictionary<int, Dictionary<int, Card>>();

            // Loop through each rarity level and create a dictionary for each rarity
            for (int j = 1; j <= 4; j++)
            {
                Dictionary<int, Card> cards = new Dictionary<int, Card>();
                int tempCount = 0;
                foreach(Card card in cardDatabase){
                    if(card.cardLevel == i && card.cardRarity == j){
                        cards.Add(tempCount, card);
                        tempCount++;
                    }
                }
                rarityDictionary.Add(j, cards);
            }
            cardDictionary.Add(i, rarityDictionary);
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
        player = singleton.player;
    }

    //Create Enemy
    public void CreateEnemy()
    {
        for(int i = 0; i < 1; i++)
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

    public void ResetPlayerStats(){
        player.strength = 0;
        player.block = 0;
        player.vulnerable = 0;
        player.weak = 0;
    }

}




