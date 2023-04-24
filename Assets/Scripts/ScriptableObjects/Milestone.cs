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
    public bool isLocked = true;
}
