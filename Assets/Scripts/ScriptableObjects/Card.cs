using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{

    [Header("Card Details")]
    public int cardID;
    public string cardName = "New Card";
    public string cardType;
    public string cardDescription;
    public int cardCost;
    public int cardLevel;
    public int cardRarity;
    public bool needsTarget = false;
    public CraftingRecipe recipe;

    [Header("Visuals")]
    public Sprite cardImage;
    public Sprite cardOverlay;

    [Header("Non Power Card Info")]
    public List<string> actionList = new List<string>();
    public int block;
    public int attack;
    public int multiAction = 1;
    public int vulnerable;
    public int weak;
    public int strength;

    [Header("Container Variables")]
    public int deckQty;
    public GameState gameState;

    [Header("Crafting")]
    public int quantity;
    public int price;

    [Header("Power Card Details")]
    public string phase;

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
                default:
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
            return cardDescription;
        }
    }

    public virtual void Effect()
    {

    }
}
