using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerTest : MonoBehaviour, IPointerClickHandler
{
    void Start()
    {
        Debug.Log("Event Trigger Test Loaded");
    }

    public void OnPointerClick(PointerEventData eventData){
        Debug.Log("OnPOinterClick Worked");

    }
    void Update()
    {
        
    }

    public void Test()
    {
        Debug.Log("Clicked");
    }
}
