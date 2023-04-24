using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]

public class EnemyObject : ScriptableObject
{

    [SerializeField] public List<EnemyAction> actionList = new List<EnemyAction>();

    public int ID;
    public string enemyName;

    public int minDay;
    public int maxDay;
    public int maxHealth;
    public int health;
    public int groupMin;
    public int groupMax;
    public Sprite enemyImage;

    [HideInInspector]
    public string rarity = "common";
    [HideInInspector]
    public string difficulty = "easy";

    

}
