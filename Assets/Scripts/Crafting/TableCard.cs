using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TableCard : CraftCard
{
    public override void Render()
    {
        CardBehavior cardBehavior = gameObject.GetComponent<CardBehavior>();
        cardBehavior.RenderCard(card, true);
        cardBehavior.quantity.text = qty.ToString();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        craft.AddToInventory(card);
        craft.RemoveFromList(craft.tableCards, this);
        base.OnPointerClick(eventData);
    }
}
