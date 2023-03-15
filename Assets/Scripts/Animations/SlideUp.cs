using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SlideUp : MonoBehaviour
{
    
    public RectTransform map;

    void Start()
    {
        map = gameObject.GetComponent<RectTransform>();
        map.DOAnchorPos(new Vector2(0, (Screen.height/2 - 650)), 1f);
    }



}
