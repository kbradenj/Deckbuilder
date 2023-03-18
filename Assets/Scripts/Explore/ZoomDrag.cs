using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomDrag : MonoBehaviour
{


    public GameObject map;
    bool isGrabbed = false;
    RectTransform rectTransform;
    float offsetX;
    float offsetY;
    Vector2 grabPosition;
    Vector2 mapInitialPosition;
    float cameraWidth;
    float widthSpace;
    float heightSpace;

    //Camera
    private Camera mainCamera;
   

    void Start()
    {
        map = GameObject.Find("Map");
        rectTransform = gameObject.GetComponent<RectTransform>();
        mainCamera = Camera.main;
        cameraWidth = Screen.width;  
        widthSpace = (rectTransform.sizeDelta.x/2) - map.transform.position.x;
        heightSpace = (rectTransform.sizeDelta.y/2) - map.transform.position.y;
        mapInitialPosition = new Vector2 (map.transform.position.x, map.transform.position.y);
    }

    void Update()
    {
        if(isGrabbed)
        {
            float xMin = mapInitialPosition.x - widthSpace;
            float xMax = rectTransform.sizeDelta.x / 2f;
            float yMin = mapInitialPosition.y - heightSpace;
            float yMax = rectTransform.sizeDelta.y / 2f;
            float xInput = Mathf.Clamp(Input.mousePosition.x - offsetX, xMin , xMax);
            float yInput = Mathf.Clamp(Input.mousePosition.y - offsetY, yMin , yMax);
            map.transform.position = new Vector2(xInput, yInput);
            mainCamera.transform.position = Input.mousePosition;
        }
    }


   public void GrabMap()
    {
        isGrabbed = true;
        grabPosition = Input.mousePosition;

        offsetX = grabPosition.x - map.transform.position.x;
        offsetY = grabPosition.y - map.transform.position.y;

    }

    public void ReleaseMap()
    {
        isGrabbed = false;  
            Debug.Log(isGrabbed);
    }


}
