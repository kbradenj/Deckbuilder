using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Slides : MonoBehaviour
{
    
    public RectTransform itemToMove = null;

    void Start()
    {
        if(SceneManager.GetActiveScene().name == "Night")
        {
            itemToMove = gameObject.GetComponent<RectTransform>();
            itemToMove.DOAnchorPos(new Vector2(0, (Screen.height/2 - 650)), 1f);
        }
    }

}
