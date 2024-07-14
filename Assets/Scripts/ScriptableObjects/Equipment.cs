using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Equipment")]
public class Equipment : ScriptableObject
{
    public int id;
    public string equipmentName;
    public Equipment equipment;
    public Sprite image;
    public List<Card> equipmentCards = new List<Card>();
}
