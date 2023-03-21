using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomDrag : MonoBehaviour
{
    public GameObject map;
    bool isGrabbed = false;
    bool isZooming = false;
    RectTransform rectTransform;
    float offsetX;
    float offsetY;
    Vector2 grabPosition;
    Vector2 mapInitialPosition;
    Vector2 mapScale;
    float scaleOffset = 1;
    float edgeWidthScale;
    float edgeHeightScale;
    
    void Start()
    {
        map = GameObject.Find("Map");
        rectTransform = gameObject.GetComponent<RectTransform>();
        mapInitialPosition = map.transform.position;
        edgeWidthScale = Screen.width / rectTransform.sizeDelta.x;
        edgeHeightScale = Screen.height / rectTransform.sizeDelta.y;
    }

    void Update()
    {
        if(isGrabbed && scaleOffset > edgeHeightScale)
        {
            float xMin = mapInitialPosition.x - (((rectTransform.sizeDelta.x * scaleOffset) / 2f) - mapInitialPosition.x + 100); 
            float yMin = mapInitialPosition.y - (((rectTransform.sizeDelta.y * scaleOffset) / 2f) - mapInitialPosition.y + 100); 

            float xMax = (rectTransform.sizeDelta.x * scaleOffset) / 2f + 100;
            float yMax = (rectTransform.sizeDelta.y * scaleOffset) / 2f + 100;

            float xInput = Mathf.Clamp(Input.mousePosition.x - offsetX, xMin , xMax);
            float yInput = Mathf.Clamp(Input.mousePosition.y - offsetY, yMin , yMax);

            if (scaleOffset > edgeWidthScale)
            {
                 map.transform.position = new Vector2(xInput, yInput);
            }
            else {
                map.transform.position = new Vector2(mapInitialPosition.x, yInput);
            }   
        }

        if(isZooming){
             if(scaleOffset > 2){
                scaleOffset = 2;
            }
            if(scaleOffset < .5)
            {
                scaleOffset = .5f;
            }
            map.transform.localScale = new Vector2(scaleOffset, scaleOffset);
            if(Input.GetAxisRaw("Mouse ScrollWheel") < 0 && scaleOffset > 0.5f)
            {
                float scaleDifference = 0.5f;
                if(scaleOffset > edgeWidthScale)
                {
                    scaleDifference = scaleOffset - edgeWidthScale;
                }

                Vector2 currentPos = map.transform.position;
                float xDifference = currentPos.x - mapInitialPosition.x;
                float yDifference = currentPos.y - mapInitialPosition.y;
                float xResult = map.transform.position.x + ((xDifference / scaleDifference) * Input.GetAxisRaw("Mouse ScrollWheel"));
                float yResult = map.transform.position.y + ((yDifference / scaleDifference) * Input.GetAxisRaw("Mouse ScrollWheel"));
                map.transform.position = new Vector2 (xResult, yResult);
            }
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
    }

    public void Zoom()
    {
        scaleOffset += Input.GetAxisRaw("Mouse ScrollWheel");
        if(Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            isZooming = true;
        }
        else
        {
            isZooming = false;
        }
    }
}
