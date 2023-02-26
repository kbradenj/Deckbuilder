using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingMaterialBehavior : MonoBehaviour
{
    public bool onTable = false;
    public GameObject craftingTable;
    public int amount;
    public string materialName;
    public Image materialImage;
    public Craft craft;
    public Card card;
    public GameObject cardPrefab;

    void Awake()
    {
        craft = GameObject.FindObjectOfType<Craft>();
        craftingTable = GameObject.Find("Crafting Area");
        amount = 3;
    }
    public void Move()
    {
            if(craft.tableMaterials.TryGetValue(materialName, out amount))
            {
                craft.AddToTable(materialName);
            }
            else{
                GameObject usedMaterial = GameObject.Instantiate(cardPrefab, new Vector2 (0,0), Quaternion.identity);
                usedMaterial.transform.localScale *= 0.5f;
                usedMaterial.GetComponent<CardBehavior>().RenderCard(card);
                usedMaterial.transform.SetParent(craftingTable.transform);
                onTable = true;
                craft.AddToTable(card.cardName);
            }

        }
    }
