using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Card))]
public class CardEditor : Editor
{
    public enum CardType
    {
        Power, Attack, Ability, Equipment
    }

    public CardType thisCardType;

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardID"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardDescription"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardCost"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardLevel"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardRarity"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("needsTarget"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("recipe"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardImage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cardOverlay"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("price"));

        switch(serializedObject.FindProperty("cardType").stringValue)
        {
            case "power":
            thisCardType = CardType.Power;
            break;

            case "attack":
            thisCardType = CardType.Attack;
            break;

            case "ability":
            thisCardType = CardType.Ability;
            break;

            case "equipment":
            thisCardType = CardType.Equipment;
            break;
        }

        thisCardType = (CardType) EditorGUILayout.EnumPopup("CardType", thisCardType);
        EditorGUILayout.Space();

        switch(thisCardType)
        {
            case CardType.Power:
            serializedObject.FindProperty("cardType").stringValue = "power";
            PowerCard();
            break;

            case CardType.Attack:
            serializedObject.FindProperty("cardType").stringValue = "attack";
            NonPowerCard();
            break;

            case CardType.Ability:
            serializedObject.FindProperty("cardType").stringValue = "ability";
            NonPowerCard();
            break;

            case CardType.Equipment:
            serializedObject.FindProperty("cardType").stringValue = "equipment";
            NonPowerCard();
            break;
        }
        
        serializedObject.ApplyModifiedProperties();
    }

    void NonPowerCard()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("actionList"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("block"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("attack"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("multiAction"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("vulnerable"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("weak"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("strength"));
    }

    void PowerCard()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("phase"));      
    }

}

[CustomEditor(typeof(Drawmageddon))]
public class PowerCardEditor : CardEditor
{
   
}