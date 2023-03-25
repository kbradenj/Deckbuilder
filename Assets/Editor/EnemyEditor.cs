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


        thisRarity = (Rarity) EditorGUILayout.EnumPopup("Rarity", thisRarity);
        EditorGUILayout.Space();

        switch(thisRarity)
        {
            case Rarity.Common:
            rarity.stringValue = "Common";
            break;
            case Rarity.Uncommon:
            rarity.stringValue = "Uncommon";
            break;
            case Rarity.Rare:
            rarity.stringValue = "Rare";
            break;
            case Rarity.Legendary:
            rarity.stringValue = "Legendary";
            break;
            case Rarity.Mythic:
            rarity.stringValue = "Mythic";
            break;

        }
        
        
   
        serializedObject.ApplyModifiedProperties();
    }

}
