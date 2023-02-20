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
    public int cardID;
    public string cardName = "New Card";
    public string cardDescription;
    public int cardCost;
    public int block;
    public int attack;
    public int multiAction = 1;
    public int vulnerable;
    public int weak;

    public bool needsTarget = false;

    public Sprite cardImage;
}
