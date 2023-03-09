using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScavengeCard : MonoBehaviour, IPointerClickHandler
{
    public Scavenge scavengeScript;

    void Start()
    {
        
    }

     public void OnPointerClick(PointerEventData eventData)
    {
        scavengeScript.SelectScavengeCard(this.gameObject);
    }
}
