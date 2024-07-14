using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{
   public Equipment selectedEquipment;
   public GameObject equipmentPrefab;
   public GameObject cardPrefab;
   public GameObject equipmentArea;
   public GameObject cardArea;
   public Equipment[] equipmentDatabase;
   private List<GameObject> equipmentObjects = new List<GameObject>();
   private List<Equipment> equipmentOptions = new List<Equipment>();
   private List<GameObject> cardObjects = new List<GameObject>();

    void Start()
    {
       LoadEquipmentDatabase();
       LoadEquipmentOptions();
    }

    private void LoadEquipmentDatabase()
    {
        equipmentDatabase = Resources.LoadAll<Equipment>("Equipment");
    }

    private void LoadEquipmentOptions(int amount = 3)
    {
         for(int i = 0; i < amount; i++)
        {
            GameObject equipmentObject = GameObject.Instantiate(equipmentPrefab, Vector2.zero, Quaternion.identity);
            equipmentObject.transform.SetParent(equipmentArea.transform);
            EquipmentBehavior newEquipment = equipmentObject.GetComponent<EquipmentBehavior>();
            newEquipment.equipment = equipmentDatabase[i];
            newEquipment.RenderEquipment();
            equipmentObjects.Add(equipmentObject);
        }
    }

    public void LoadEquipmentCards(Equipment e)
    {
        foreach(Card card in e.equipmentCards)
        {
            GameObject newCard = GameObject.Instantiate(cardPrefab, Vector2.zero, Quaternion.identity);
            CardBehavior newCardBehavior = newCard.GetComponent<CardBehavior>();
            cardObjects.Add(newCard);
            newCard.transform.SetParent(cardArea.transform);
            newCardBehavior.RenderCard(card);
        }
        cardArea.GetComponent<Image>().color = Color.white;
    }

    public void ClearAllEquipmentCards()
    {
        foreach(GameObject obj in cardObjects)
        {
            Destroy(obj);
        }
    }

    public void DeselectAll()
    {
        foreach(GameObject equipmentObj in equipmentObjects)
        {
            equipmentObj.GetComponent<EquipmentBehavior>().Deselect();
        }
    }

    public void ChooseEquipment()
    {
        Debug.Log($"You chose {selectedEquipment.equipmentName}");
    }
}
