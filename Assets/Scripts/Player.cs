using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : Character
{
  //TMP
  public TMP_Text defenseText;
  public TMP_Text healthText;
  public TMP_Text maxHealthText;
  public TMP_Text actionPointsField;
  public Slider healthSlider;

  //Lists
  public List<Card> startingDeck;

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
    health = 200;
    maxHealth = 100;
  }

  void Start()
  {
    
  }

  //Update UI Text Fields
  public override void UpdateStats()
  {
    defenseText.text = block.ToString();
    healthText.text = health.ToString();
    maxHealthText.text = "/" + maxHealth.ToString();
    healthSlider.value = ((float)health/maxHealth) * 100;
    actionPointsField.text = turnAP.ToString(); 
  }

  public override void UpdateStatus()
  {
    foreach (StatusIcon icon in statusIcons){
      switch(icon.type)
      {
        case "weak":
        icon.statusTextContainer.text = weak.ToString();
        break;
        case "strength":
        icon.statusTextContainer.text = strength.ToString();
        break;
        case "vulnerable":
        icon.statusTextContainer.text = vulnerable.ToString();
        break;
      }
    }
  }
  //Battle States
  public void EndTurn()
  {
    turnAP = ap; //might want to put in start turn rather than end turn
    UpdateStats();
  }

  //PlayerDeath
  public override void Death()
    {
        GameObject.FindObjectOfType<GameState>().isBattle = false;
        Destroy(this.gameObject);
        SceneManager.LoadScene("DeathScreen");
    }

}
