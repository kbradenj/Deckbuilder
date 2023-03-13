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

    public Craft craft;


    void Awake()
    {
        image = gameObject.GetComponentInChildren<Image>();
        qtyText = gameObject.GetComponentInChildren<TMP_Text>();
        craft = GameObject.FindObjectOfType<Craft>();
    }

      public virtual void OnPointerClick(PointerEventData eventData)
    {
        craft.RemoveFromList("inventory", card.cardName);
    }

    public virtual void Render()
    {
        qtyText.text = qty.ToString();
        image.sprite = card.cardImage;   
    }
}
