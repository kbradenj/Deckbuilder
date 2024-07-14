using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

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
  public int drawSize = 0;
  public bool isPlayerTurn = true;
  public int attackCardsPlayed = 0;
  public int skillCardsPlayed = 0;
  public int powerCardsPlayed = 0;
  
  //Temp Stats
  public int turnAP;

  //Artifacts
  public bool totemOfFury = false;

  public override void EndTurn()
  {
    base.EndTurn();
    isPlayerTurn = false;
    attackBoost = 0;
    RemoveStatusIcon("attackBoost");

  }

  public void SetUpBattle()
  {
    singleton = FindObjectOfType<Singleton>();
    actionPointsField = GameObject.Find("Action Points Amount").GetComponent<TMP_Text>();
    gameState = FindObjectOfType<GameState>();
    cardManager = FindObjectOfType<CardManager>();
    healthText = GameObject.Find("Player Health Text").GetComponent<TMP_Text>();
    defenseText = GameObject.Find("Player Defense Amount").GetComponent<TMP_Text>();
    healthSlider = GameObject.Find("Player Health Slider").GetComponent<Slider>();
    statusIconsArea = GameObject.Find("Player Status Icons");
  }

public int GetPlayerTurnAP()
{
  return turnAP;
}
  public override void StartTurn()
  {
    block = halfBlock ? block/=2 : 0;
    isPlayerTurn = true;
    turnAP = ap;
    if(fear > 0)
    {
      turnAP--;
    }
    base.StartTurn();
    cardManager.Discard();
    cardManager.Draw(drawSize, true);
    if(shriek > 0)
    {
        cardManager.ShriekCards();
    }
    if(gameState.powerCards.Count > 0)
    {
      foreach(KeyValuePair<string, Card> kvp in gameState.powerCards["turnStart"])
      {
        gameState.powerCards["turnStart"][kvp.Key].Effect();
      }
    }
  }

  //Update UI Text Fields
  public override void UpdateStats()
  {
    if(actionPointsField == null)
    {
      actionPointsField = GameObject.Find("Action Points Amount").GetComponent<TMP_Text>();
    }
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

    public void IncrementCardUse(string type)
    {
      switch(type)
      {
        case "attack":
        attackCardsPlayed++;
        if(totemOfFury && attackCardsPlayed % 5 == 0)
        {
          singleton.activeArtifacts.Find(x => x.artifactName == "TotemOfFury").Activate();
        }
        break;
        case "skill":
        skillCardsPlayed++;
        break;
        case "power":
        powerCardsPlayed++;
        break;
        default:
        break;
      }
    }

}
