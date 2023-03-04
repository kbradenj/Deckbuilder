using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Character : MonoBehaviour
{
  public int block;
  public int maxHealth;
  public int health; 
  public int level = 1;
  public int vulnerable = 0;
  public int weak = 0;
  public float weaknessMod = 1f;
  public int strength = 0;

  public TMP_Text statusText;

  public GameObject statusIconPrefab;
  public GameState gameState;

  public List<StatusIcon> statusIcons = new List<StatusIcon>();

  //Game Objects
  public GameObject statusIconsArea;
  public GameObject statusIcon;
  public GameObject[] statuses;
  public Image statusImage;

  public virtual void UpdateStats(){}
  public virtual void UpdateStatus(){}
  public virtual void AdjustStatus(){}
  public virtual void RemoveStatusIcon(string icon){}
  public virtual void NextTurn(){}
  public virtual void Death(){}

  void Awake(){
    gameState = GameObject.FindObjectOfType<GameState>();
  }

  public  void AddStatusIcon(string icon, int amount)
  {
    statusIconsArea = transform.Find("Status Icons").gameObject;
    statusIcon = Instantiate(statusIconPrefab, new Vector2 (0, 0), Quaternion.identity);
    statusIcon.transform.SetParent(statusIconsArea.transform, false);

    statusImage = statusIcon.GetComponent<Image>();
    statusImage.sprite = Resources.Load<Sprite>("StatusIcons/" + icon);

    statusText = statusIcon.GetComponentInChildren<TMP_Text>();
    statusText.text = amount.ToString();

    StatusIcon statusIconItem = new StatusIcon {type = icon, statusAmount = amount, statusIconContainer = statusIcon, statusTextContainer = statusText};
    statusIcons.Insert(0, statusIconItem);
  }
  
  public class StatusIcon 
  {
    public string type {get; set;}
    public int statusAmount {get; set;}
    public GameObject statusIconContainer {get; set;}
    public TMP_Text statusTextContainer {get; set;}
  }
}
