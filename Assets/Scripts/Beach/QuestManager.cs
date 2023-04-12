using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

public GameObject questLogPrefab;


public void OpenQuestLog()
    {
        GameObject questLog = GameObject.Instantiate(questLogPrefab, new Vector2(Screen.width/2f, Screen.height/2f), Quaternion.identity);
        questLog.transform.SetParent(GameObject.Find("Main Canvas").transform);
    }


}