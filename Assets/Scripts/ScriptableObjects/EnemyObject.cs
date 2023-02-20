using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]

public class EnemyObject : ScriptableObject
{

    [SerializeField] public List<EnemyAction> actionList = new List<EnemyAction>();

    public int ID;
    public string enemyName;
    public int level;
    public int health;

    public Sprite enemyImage;

}
