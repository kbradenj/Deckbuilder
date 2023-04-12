using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyObject))]
public class EnemyEditor : Editor
{
    public enum Rarity
    {
        Common, Uncommon, Rare, Legendary, Mythic
    }

    public Rarity thisRarity;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SerializedProperty rarity = serializedObject.FindProperty("rarity");

        switch(serializedObject.FindProperty("rarity").stringValue)
        {
            case "commmon":
            thisRarity = Rarity.Common;
            break;
            case "uncommon":
            thisRarity = Rarity.Uncommon;
            break;
            case "rare":
            thisRarity = Rarity.Rare;
            break;
            case "legendary":
            thisRarity = Rarity.Legendary;
            break;
            case "mythic":
            thisRarity = Rarity.Mythic;
            break;
        }

        thisRarity = (Rarity) EditorGUILayout.EnumPopup("Rarity", thisRarity);
        EditorGUILayout.Space();

        switch(thisRarity)
        {
            case Rarity.Common:
            rarity.stringValue = "common";
            break;
            case Rarity.Uncommon:
            rarity.stringValue = "uncommon";
            break;
            case Rarity.Rare:
            rarity.stringValue = "rare";
            break;
            case Rarity.Legendary:
            rarity.stringValue = "legendary";
            break;
            case Rarity.Mythic:
            rarity.stringValue = "mythic";
            break;
        }
        
        serializedObject.ApplyModifiedProperties();

    }

}
