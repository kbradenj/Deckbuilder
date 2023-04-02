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
    public int weak = 0;
    public int vulnerable = 0;
    public float vulnerableMod = 1.25f;
    public float weaknessMod = 1f;
    public int strength = 0;
    public int baseStrength;
    public int baseDexterity;
    public int bonusDraw;
    public int poison;

    //Game Objects
    public GameObject statusIconsArea;
    public GameObject statusIcon;
    public GameObject[] statuses;
    public Image statusImage;

    //Scripts
    public ActionManager actions;
    public CardManager cardManager;

    //Child Class Methods
    public virtual void UpdateStats(){}
    public virtual void NextTurn(){}
    public virtual void Death(){}

    void Awake(){
		gameState = GameObject.FindObjectOfType<GameState>();
		cardManager = FindObjectOfType<CardManager>();
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
			if(vulnerable > 1)
			{
				vulnerable --;
			}
				else if(vulnerable == 1)
			{
				vulnerable --;
				RemoveStatusIcon("vulnerable");
			}

			if(weak > 1)
			{
				weak --;
			}
			else if(weak == 1)
			{
				weak --;
				RemoveStatusIcon("weak");
				weaknessMod = 1;
			}

			if(poison > 1)
			{
				poison --;
			}
			else if(poison == 1)
			{
				poison --;
				RemoveStatusIcon("poison");
			}

			UpdateStatus();
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
			}
		}
    }

    public void AddStatusIcon(string icon, int amount)
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

    public virtual void RemoveStatusIcon(string icon){
		foreach(StatusIcon status in statusIcons)
		{
			if(status.type == icon){
				
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
