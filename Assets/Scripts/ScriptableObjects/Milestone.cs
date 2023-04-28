using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Milestone", menuName = "Milestone")]
public class Milestone : ScriptableObject
{
    public int day;
    public string type;
    public string identifier;
    public int amount;
    public Sprite image;
    [TextArea(0,8)]
    public string description;
    public Card card = null;
    public bool isLocked = true;
}
