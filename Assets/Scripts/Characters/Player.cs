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

  //Player Stats
  public int ap = 4;
  public int drawSize = 5;
  
  //Temp Stats
  public int turnAP;

  void Awake()
  {
    bonusDraw = 0;
    actionPointsField = GameObject.Find("Action Points Amount").GetComponent<TMP_Text>();
    gameState = GameObject.FindObjectOfType<GameState>();
  }

  public override void StartTurn()
  {
    turnAP = ap;
    base.StartTurn();
    cardManager.Draw(drawSize);
    foreach(KeyValuePair<string, Card> kvp in gameState.powerCards["turnStart"])
    {
      gameState.powerCards["turnStart"][kvp.Key].Effect();
    }
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

  //PlayerDeath
  public override void Death()
    {
        GameObject.FindObjectOfType<GameState>().isBattle = false;
        Destroy(this.gameObject);
        SceneManager.LoadScene("DeathScreen");
    }

}
