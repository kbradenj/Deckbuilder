using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Battle : MonoBehaviour
{
    private Singleton singleton;
    private GameState gameState;
    private CardManager cardManager;

    //Prefabs
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject artifactPrefab;

    //Areas
    public GameObject playerArea;
    public GameObject enemyArea;
    public GameObject artifactArea;

    public List<Enemy> enemies;
    public EnemyObject[] enemyDatabase;
    public EnemyAction[] enemyActionDatabase;

    public EnemyObject forcedEnemy;
    public int forcedDay = 0;
    public string forcedRarity = null;
    public string forcedDifficulty = null;

    public Player player;

    private bool enemiesLoaded = false;

    //Counters
    public int numOfEnemies = 0;
    public int enemyAmount = 2;

    public Dictionary<string, Dictionary<string, Card>> powerCards;
    

    void Start()
    {
        singleton = GameObject.FindObjectOfType<Singleton>();
        gameState = FindObjectOfType<GameState>();
        powerCards = gameState.powerCards;

        //Set up Card Manager
        cardManager = FindObjectOfType<CardManager>();
        powerCards = new Dictionary<string, Dictionary<string, Card>>();
        playerArea = GameObject.Find("Player Area");
        enemyArea = GameObject.Find("Enemy Area");
        artifactArea = GameObject.Find("Artifact Area");
        gameState.isBattle = true;
        LoadEnemyDatabase();
        StartBattle();
    }

    void Update() {
        //Is the battle over?
        if(numOfEnemies == 0 && enemiesLoaded){
            DOTween.KillAll();
            singleton.player.health = player.health;
            gameState.isBattle = false;
            ResetPlayerStats();
            singleton = GameObject.FindObjectOfType<Singleton>();
            singleton.isBattle = false;
            singleton.navigation.Navigate("WinScreen");
        }
    }
   

   public void LoadArtifacts()
   {
        if(singleton.activeArtifacts.Count > 0)
        {
            foreach(Artifact artifact in singleton.activeArtifacts)
            {
                GameObject artifactIcon = GameObject.Instantiate(artifactPrefab, Vector2.zero, Quaternion.identity);
                DisplayArtifact displayArtifact = artifactIcon.GetComponent<DisplayArtifact>();
                displayArtifact.thisArtifact = artifact;
                artifactIcon.transform.SetParent(artifactArea.transform);
                displayArtifact.RenderArtifact();
                artifact.Effect();
                
            }
        }
   }
   //Load Enemies
    public void LoadEnemyDatabase()
    {
        if(singleton.enemyDatabase.Length <= 0)
        {
            enemyDatabase = Resources.LoadAll<EnemyObject>("Enemies");
            singleton.enemyDatabase = enemyDatabase;
            LoadEnemyCatalog();
        }
    }

    public void LoadEnemyCatalog()
    {
        string[] difficulty = {"easy", "normal", "hard", "elite", "insane"};
        for(int i = 1; i <= 25; i++)
        {
            Dictionary<string, List<EnemyObject>> newDictionary = new Dictionary<string, List<EnemyObject>>();
            foreach(string thisDifficulty in difficulty)
            {
                List<EnemyObject> newList = new List<EnemyObject>();
                newDictionary.Add(thisDifficulty, newList);
            }
            singleton.enemyCatalog.Add(i, newDictionary);
        }
         foreach(EnemyObject enemy in singleton.enemyDatabase)
            {
                for(int i = enemy.minDay; i <= enemy.maxDay; i++)
                {
                    singleton.enemyCatalog[i][enemy.difficulty].Add(enemy);
                }
            }
    }


    //Battle Starts
    public void StartBattle()
    {
        singleton.isBattle = true;
        CreatePlayer();
        CreateEnemy(GetRandomEnemy(GetRandomRarity()));
        player.SetUpBattle();
        player.StartTurn();
        player.UpdateStats();
        cardManager.LoadPlayerDeck();
        LoadArtifacts();
        foreach(Enemy enemy in enemies)
        {
            enemy.NextTurn();
        }
    }

      //Create Player
    public void CreatePlayer()
    {
        GameObject playerObject = GameObject.Instantiate(playerPrefab, new Vector2(0,0), Quaternion.identity) as GameObject;
        playerObject.transform.SetParent(playerArea.transform, false); 
        player = singleton.player;
    }

    //Determine Enemy
     private EnemyObject GetRandomEnemy(string rarity, string difficulty = "easy")
    {
        if(forcedEnemy != null)
        {
            return forcedEnemy;
        }

        //if first attempt to get enemy fails, we check for a common rarity match, if fails again, we check for a common, easy enemy
        bool checkedForCommon = false;
        if(singleton.currentPathChoice != null)
        {
            difficulty = singleton.currentPathChoice.difficulty;
        }
        List<EnemyObject> possibleEnemies = new List<EnemyObject>();
       
        if(forcedDay != 0)
        {
            singleton.dayCount = forcedDay;
        }

        // if(forcedRarity != null)
        // {
        //     Debug.Log("Forced rarity is NOT Null");
        //     rarity = forcedRarity;
        // }

        // if(forcedDifficulty != null)
        // {
        //     Debug.Log("Forced rarity is NOT Null");
        //     difficulty = forcedDifficulty;
        //     if(singleton.enemyCatalog[singleton.dayCount][difficulty] == null)
        //     {
        //         Debug.Log("Forced difficulty not found in dictionary");
        //         return null;
        //     }
        // }

        foreach(EnemyObject enemy in singleton.enemyCatalog[singleton.dayCount][difficulty])
        {
            if (enemy.rarity == rarity)
            {
                possibleEnemies.Add(enemy);
            }
        }

        if(possibleEnemies.Count > 0)
        {
            return possibleEnemies[Random.Range(0,possibleEnemies.Count)];
        }
        else
        {
            if(checkedForCommon)
            {
                return GetRandomEnemy("common", "easy");
            }
            else
            {
                checkedForCommon = true;
                return GetRandomEnemy("common", difficulty);
            }

        }
    }

    private string GetRandomRarity()
    {
        float randomNum = Random.Range(0f, 100f);

        switch(randomNum)
        {
            case var expression when randomNum < gameState.mythicChance:
            return "mythic";
            case var expression when randomNum < gameState.legendaryChance:
            return "legendary";
            case var expression when randomNum < gameState.rareChance:
            return "rare";
            case var expression when randomNum < gameState.uncommonChance:
            return "uncommon";
            default:
            return "common";
        }
    }

    //Create Enemy
    public void CreateEnemy(EnemyObject enemyObject)
    {
        int randomEnemyAmount = Random.Range(enemyObject.groupMin, enemyObject.groupMax);
        for(int i = 0; i < randomEnemyAmount; i++)
        {
            Vector2 enemyAreaPosition = enemyArea.transform.position;
            float enemyAreaWidth = enemyArea.GetComponent<RectTransform>().sizeDelta.x;
            float startPosition = Screen.width - 200;
            float enemySpace = Mathf.Clamp(enemyAreaWidth/randomEnemyAmount, 100, 300);
            GameObject enemyNew = GameObject.Instantiate(enemyPrefab, new Vector2(startPosition - (i * enemySpace), enemyArea.transform.position.y * .75f), Quaternion.identity) as GameObject;
            enemyNew.transform.SetParent(enemyArea.transform);
            Enemy thisEnemy = enemyNew.GetComponent<Enemy>();
            thisEnemy.enemy = enemyObject;
            thisEnemy.strength = singleton.dayCount - 1;
            thisEnemy.weaknessMod = 1f;
            BoxCollider2D collider2D = enemyNew.GetComponent<BoxCollider2D>();
            collider2D.size = new Vector2(enemySpace, 200);
            enemies.Add(thisEnemy);
            numOfEnemies += 1;
        }
        enemiesLoaded = true;
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
        player.damageReduction = 0;
        player.attackCardsPlayed = 0;
        singleton.dayLeft = singleton.maxDaylight;
    }

    //DEBUG

     public void ShowEnemyCatalogInConsole()
    {
        foreach(KeyValuePair<int, Dictionary<string, List<EnemyObject>>> kvp in singleton.enemyCatalog)
        {
            Debug.Log("day: " + kvp.Key);
            foreach(KeyValuePair<string, List<EnemyObject>> kvp2 in kvp.Value)
            {
                Debug.Log("difficulty: " + kvp2.Key);
                foreach(EnemyObject enemy in kvp2.Value)
                {
                    Debug.Log(enemy.enemyName + " : Rarity: " + enemy.rarity);
                }
            }  
        }
    }

    public Enemy TargetRandomEnemy()
    {
        return enemies[Random.Range(0, enemies.Count)];
    }
}
