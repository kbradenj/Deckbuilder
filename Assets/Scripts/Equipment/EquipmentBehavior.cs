using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class EquipmentBehavior : MonoBehaviour
{
    bool selected = false;
    public TMP_Text equipmentTitle;
    public Image equipmentBG;
    public Image equipmentImage;
    public Equipment equipment;
    private EquipmentManager equipmentManager;

    void Awake()
    {
        equipmentManager = GameObject.FindObjectOfType<EquipmentManager>();
    }
    public void Select()
    {
        if(!selected)
        {   equipmentManager.DeselectAll();
            equipmentManager.ClearAllEquipmentCards();
            equipmentManager.selectedEquipment = equipment;
            equipmentBG.color = Color.white;
            equipmentTitle.color = Color.black;
            equipmentManager.LoadEquipmentCards(equipment);
            selected = true;
        }
    }

    public void Deselect()
    {
        equipmentBG.color = new Color(0,0,0,0);
        equipmentTitle.color = Color.white;
        selected = false;
    }

    public void RenderEquipment()
    {
        equipmentTitle.text = equipment.equipmentName;
        equipmentImage.sprite = equipment.image;
    }
}
