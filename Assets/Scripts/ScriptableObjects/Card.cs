using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{

    [Header("Card Details")]
    public int cardID;
    public bool isLocked = false;
    public string cardName = "New Card";
    public string cardType;
    public List<string> descriptionParts = new List<string>();
    public int cardCost;
    public int cardLevel;
    public int cardRarity;
    public bool needsTarget = false;
    public CraftingRecipe recipe;

    [Header("Visuals")]
    public Sprite cardImage;
    public Sprite cardOverlay;
    private string boosted = "green";
    private string reduced = "red";

    [Header("Non Power Card Info")]
    public List<string> actionList = new List<string>();
    public int block;
    public int attack;
    public int draw;
    public int multiAction = 1;
    public int vulnerable;
    public int weak;
    public int strength;
    public int evade;

    public int modDamage;
    public int modBlock;
    public int attackBoost;

    [Header("Container Variables")]
    public int deckQty;
    public GameState gameState;

    [Header("Crafting")]
    public int quantity;
    public int price;

    [Header("Power Card Details")]
    public string phase;

    public string FormatString()
    {
        List<string> attributes = new List<string>();
        string outputString = "";
        foreach(string action in actionList){
            switch(action)
            {
                case "attack":
                case "xattack":
                case "attackall":
                    if(modDamage > attack)
                    {
                        attributes.Add("<color=" + boosted + ">" + modDamage + "</color>");
                    }
                    else if(modDamage < attack)
                    {
                        attributes.Add("<color=" + reduced + ">" + modDamage + "</color>");
                    }
                    else
                    {
                        attributes.Add(modDamage.ToString());
                    }
                break;
                case "block":
                case "xblock":
                    if(modBlock > block)
                    {
                        attributes.Add("<color=" + boosted + ">" + modBlock + "</color>");
                    }
                    else if(modBlock < block)
                    {
                        attributes.Add("<color=" + reduced + ">" + modBlock + "</color>");
                    }
                    else
                    {
                        attributes.Add(modBlock.ToString());
                    }
                break;
                case "vulnerable":
                attributes.Add(vulnerable.ToString());
                break;
                case "weak":
                attributes.Add(weak.ToString());
                break;
                case "strength":
                attributes.Add(strength.ToString());
                break;
                case "evade":
                attributes.Add(evade.ToString());
                break;
                case "draw":
                attributes.Add(draw.ToString());
                break;
                case "attackBoost":
                attributes.Add(attackBoost.ToString());
                break;
                default:
                break;
            }
        }

        foreach(string descriptionPart in descriptionParts)
        {
            if(descriptionPart == "0")
            {
                outputString += attributes[0].ToString();
            }
            else if (descriptionPart == "1")
            {
                outputString += attributes[1].ToString();
            }
            else
            {
                outputString += descriptionPart;
            }
            outputString += " ";

        }
        return outputString;
    }

    public virtual void Effect()
    {

    }

    public void Highlight()
    {

    }
}
