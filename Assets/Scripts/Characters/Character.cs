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

    public TMP_Text statusText;

    public GameObject statusIconPrefab;
    public GameState gameState;

    public List<StatusIcon> statusIcons = new List<StatusIcon>();

    //Buffs/Debuffs
	public int attackBoost;
    public int weak;
    public int vulnerable;
    public float vulnerableMod = 1f;
    public float weaknessMod = 1f;
    public int strength;
    public int baseStrength;
	public int dexterity;
    public int baseDexterity;
    public int bonusDraw;
    public int poison;
	public int shriek;
	public int fear;
	public int evade;

    //Game Objects
    public GameObject statusIconsArea;
    public GameObject statusIcon;
    public GameObject[] statuses;
    public Image statusImage;

    //Scripts
    public ActionManager actions;
    public CardManager cardManager;
	public Singleton singleton;

    //Child Class Methods
    public virtual void UpdateStats(){}
    public virtual void NextTurn(){}
    public virtual void Death(){}

    void Awake(){
		gameState = GameObject.FindObjectOfType<GameState>();
		cardManager = FindObjectOfType<CardManager>();
		singleton = FindObjectOfType<Singleton>();
		actions = FindObjectOfType<ActionManager>();
		health = maxHealth;
    }

    public virtual void StartTurn()
    {
		block=0;

		if(health - poison <= 0)
		{
			health = 0;
			Death();
		}
		else
		{
			health -= poison;
			UpdateStats();
		}
	}

	public virtual void EndTurn()
	{
		AdjustStatus();
		UpdateStatus();
	}

	public virtual void AdjustStatus(){
		if(statusIcons == null){
			return;
		} 
		else
		{
			ReduceEffect(ref vulnerable, "vulnerable");
			ReduceEffect(ref weak, "weak");
			ReduceEffect(ref poison, "poison");
			ReduceEffect(ref shriek, "shriek");
			ReduceEffect(ref fear, "fear");

			UpdateStatus();
		}
    }

	public void ReduceEffect(ref int statusAmt, string status)
	{
		if(statusAmt > 1)
		{
			statusAmt --;
		}
		else if(statusAmt <= 1)
		{
			statusAmt = 0;
			if(status == "weak")
			{
				weaknessMod = 1;
			}
			else if(status == "vulnerable")
			{
				vulnerableMod = 1;
			}
			RemoveStatusIcon(status);
			NextTurn();
		}
	}

    public virtual void UpdateStatus(){
		foreach (StatusIcon icon in statusIcons)
		{
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
				case "poison":
				icon.statusTextContainer.text = poison.ToString();
				break;
				case "shriek":
				icon.statusTextContainer.text = shriek.ToString();
				break;
				case "fear":
				icon.statusTextContainer.text = fear.ToString();
				break;
				case "attackBoost" :
				icon.statusTextContainer.text = attackBoost.ToString();
				break;
				case "evade" :
				icon.statusTextContainer.text = evade.ToString();
				break;
			}
		}
    }

    public void AddStatusIcon(string icon, int amount)
    {
		statusIcon = Instantiate(statusIconPrefab, new Vector2 (0, 0), Quaternion.identity);
		statusIcon.transform.SetParent(statusIconsArea.transform, false);

		statusImage = statusIcon.GetComponent<Image>();
		statusImage.sprite = Resources.Load<Sprite>("StatusIcons/" + icon);

		statusText = statusIcon.GetComponentInChildren<TMP_Text>();
		statusText.text = amount.ToString();

		StatusIcon statusIconItem = new StatusIcon {type = icon, statusAmount = amount, statusIconContainer = statusIcon, statusTextContainer = statusText};
		statusIcons.Insert(0, statusIconItem);
    }

    public virtual void RemoveStatusIcon(string icon){
		for(int i = 0; i < statusIcons.Count; i++)
		{
			if(statusIcons[i].type == icon){
				Destroy(statusIcons[i].statusIconContainer);
				statusIcons.Remove(statusIcons[i]);
			}
		}
    }

    public class StatusIcon 
    {
		public string type {get; set;}
		public int statusAmount {get; set;}
		public GameObject statusIconContainer {get; set;}
		public TMP_Text statusTextContainer {get; set;}
    }
}
