using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayArtifact : MonoBehaviour
{

    public GameObject descriptionObject;
    public TMP_Text descriptionText;
    public Artifact thisArtifact;
    public Image image;
    public bool hasCount;
      
    void Start()
    {
        if(hasCount)
        {
            //TODO: add counter for has count artifacts
        }
    }
      
    public void ShowDescription()
    {
        descriptionObject.SetActive(true);
        descriptionText.text = thisArtifact.description;
    }

    public void HideDescription()
    {
       descriptionObject.SetActive(false);
    }

    public void RenderArtifact()
    {
        image.sprite = thisArtifact.artifactImage;
        descriptionText.text = thisArtifact.description;
    }
}
