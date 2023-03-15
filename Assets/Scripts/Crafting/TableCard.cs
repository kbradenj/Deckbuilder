using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TableCard : CraftCard
{
    public CardBehavior cardBehavior;
    public override void Awake()
    {
        base.Awake();
        cardBehavior = gameObject.GetComponent<CardBehavior>();
    }
    public override void Render()
    {
        cardBehavior.quantity.text = qty.ToString();
        cardBehavior.RenderCard(card, true);
  
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        craft.AddToInventory(card);
        craft.RemoveCard(craft.tableCards, this);
        base.OnPointerClick(eventData);
    }
}
