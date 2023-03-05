using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Stat Reward", menuName = "Stat Reward")]
public class StatReward : ScriptableObject
{
public string statType;
public int baseAmount;
public float modifier;
public Sprite sprite;
}
