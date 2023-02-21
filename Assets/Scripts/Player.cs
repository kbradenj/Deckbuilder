using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : Character
{
  //TMP
  public TMP_Text defenseText;
  public TMP_Text healthText;
  public TMP_Text actionPointsField;

  //Scripts
  public CardManager cardManager;
  public Actions actions;

  //Player Stats
  public int ap = 4;
  public int drawSize = 5;

  //Temp Stats
  public int turnAP;

  void Awake()
  {
    cardManager = FindObjectOfType<CardManager>();
    actions = FindObjectOfType<Actions>();
    actionPointsField = GameObject.Find("Action Points Amount").GetComponent<TMP_Text>();
    cardManager.LoadPlayerDeck(this);
    health = 400;
  }

  void Start()
  {
    cardManager.Draw();
  }

  //Update UI Text Fields
  public override void UpdateStats()
  {
    defenseText.text = block.ToString();
    healthText.text = health.ToString();
    actionPointsField.text = turnAP.ToString(); 
  }

  //Battle States
  public void EndTurn()
  {
    turnAP = ap; //might want to put in start turn rather than end turn
    UpdateStats();
  }

}
