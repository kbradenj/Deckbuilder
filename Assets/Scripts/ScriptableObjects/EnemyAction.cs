using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Action", menuName = "EnemyAction")]
public class EnemyAction : ScriptableObject
{
    public string id; //id # 0,1,2 etc
    public string type = "New Enemy Action"; //attack //slime //disease
    public int baseAmount; //3 (=lvl 3) //1
    public int multiAction = 1; //2 (=x2) //4 (=x4)

    //type then gets passed through to the enemyactions cs script and passed into the appropriate method

}
