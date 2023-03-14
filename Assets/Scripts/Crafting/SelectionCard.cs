using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectionCard : CraftCard
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        craft.AddToTable(this);
        craft.RemoveFromList(craft.inventoryCards, this);
    }

    public override void Render()
    {
        image.sprite = card.cardImage;
        qtyText.text = qty.ToString();
        base.Render();
    }
}
