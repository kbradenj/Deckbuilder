using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
  public int block;
  public int maxHealth;
  public int health; 
  public int level;
  public int vulnerable = 0;

  public GameObject statusIconPrefab;

  public virtual void UpdateStats(){}
  public virtual void UpdateStatus(){}
  public virtual void AddStatusIcon(string icon){}
  public virtual void RemoveStatusIcon(string icon){}

}
