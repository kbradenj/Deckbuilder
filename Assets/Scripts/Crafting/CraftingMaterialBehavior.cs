using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingMaterialBehavior : MonoBehaviour
{
    public GameObject craftingTable;
    public int amount;
    public string materialName;
    public Image materialImage;
    public Craft craft;
    public Card card;
    public GameObject cardPrefab;
    public TMP_Text materialCountText;

    void Awake()
    {
        craft = GameObject.FindObjectOfType<Craft>();
        craftingTable = GameObject.Find("Crafting Area");
        amount = 3;
    }

    public void UpdateValue(int newAmount)
    {
        materialCountText.text = newAmount.ToString();
    }

    public void Move()
    {
            if(craft.tableMaterials.TryGetValue(materialName, out amount))
            {
                craft.AddToTable(materialName);
            }
            else{
                craft.AddToTable(card.cardName);
                GameObject usedMaterial = GameObject.Instantiate(cardPrefab, new Vector2 (0,0), Quaternion.identity);
                CardBehavior cardBehavior = usedMaterial.GetComponent<CardBehavior>();
                cardBehavior.displayAmount = true;
                cardBehavior.RenderCard(card);
                usedMaterial.transform.localScale *= 0.5f;
                usedMaterial.transform.SetParent(craftingTable.transform);
            }
            int tempCount = craft.inventory[materialName];
            tempCount -= 1;
            craft.inventory[materialName] = tempCount;
            if(tempCount <= 0)
            {
                Destroy(this.gameObject);
            }
            else
            {
                UpdateValue(tempCount);
            }
            

    }
}
