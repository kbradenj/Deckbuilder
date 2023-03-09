using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using TMPro;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]

public class Card : ScriptableObject
{
    [SerializeField] public List<string> actionList = new List<string>();
    [Header("Card Details")]
    public int cardID;
    public string cardName = "New Card";
    public string cardDescription;
    public int cardLevel;
    public int cardRarity;

    [Header("Battle")]
    public int cardCost;
    public int block;
    public int attack;
    public int multiAction = 1;
    public int vulnerable;
    public int weak;
    public int strength;
    public bool needsTarget = false;

    [Header("Crafting")]
    public int quantity;
    public int price;

    [Header("Visuals")]
    public Sprite cardImage;
    public Sprite cardOverlay;

    public string FormatString(){
        List<int> attributes = new List<int>();
        foreach(string action in actionList){
            switch(action)
            {
                case "attack":
                case "xattack":
                case "attackall":
                attributes.Add(attack);
                break;
                case "block":
                case "xblock":
                attributes.Add(block);
                break;
                case "vulnerable":
                attributes.Add(vulnerable);
                break;
                case "weak":
                attributes.Add(weak);
                break;
                case "strength":
                attributes.Add(strength);
                break;
            }
        }
        switch(attributes.Count)
        {
            case 1:
            return string.Format(cardDescription, attributes[0]);
            case 2:
            return string.Format(cardDescription, attributes[0], attributes[1]);
            default:
            return "No Description";
        }
    }
}
