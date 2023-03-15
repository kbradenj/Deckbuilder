using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CraftCard : MonoBehaviour, IPointerClickHandler
{
    public int qty;
    public Card card;
    public Image image; 
    public TMP_Text qtyText;
    public bool isInstantiated = false;
    public bool isDisabled = false;

    public Craft craft;


    public virtual void Awake()
    {
        craft = GameObject.FindObjectOfType<Craft>();
    }

    public virtual void OnPointerClick(PointerEventData eventData){}
    public virtual void Render(){}
    public virtual void ToggleDisable(){}
}
