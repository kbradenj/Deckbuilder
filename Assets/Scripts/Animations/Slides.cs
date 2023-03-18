using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Slides : MonoBehaviour
{
    
    public RectTransform itemToMove;

    void Start()
    {
        if(SceneManager.GetActiveScene().name == "Night")
        {
        itemToMove = gameObject.GetComponent<RectTransform>();
        itemToMove.DOAnchorPos(new Vector2(0, (Screen.height/2 - 650)), 1f);
        }
    }

    public void SlideIn(GameObject screenToLoad)
    {
        itemToMove = screenToLoad.GetComponent<RectTransform>();
        itemToMove.DOAnchorPos(new Vector2(0, 0), 1f);
    }

    public void SlideDown(GameObject screenToLoad){
        itemToMove = screenToLoad.GetComponent<RectTransform>();
        itemToMove.DOAnchorPos(new Vector2(0, -Screen.height), 1f);
    }

    public void SlideLeft(GameObject screenToLoad){
        itemToMove = screenToLoad.GetComponent<RectTransform>();
        itemToMove.DOAnchorPos(new Vector2(-Screen.width, 0), 1f);
    }

    public void SlideUp(GameObject screenToLoad){
        itemToMove = screenToLoad.GetComponent<RectTransform>();
        itemToMove.DOAnchorPos(new Vector2(0, Screen.height), 1f);
    }

    public void SlideRight(GameObject screenToLoad){
        itemToMove = screenToLoad.GetComponent<RectTransform>();
        itemToMove.DOAnchorPos(new Vector2(Screen.width, 0), 1f);
    }



}
