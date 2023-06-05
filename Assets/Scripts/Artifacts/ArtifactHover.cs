using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArtifactHover : MonoBehaviour
{
    public GameObject descriptionPrefab;
    private GameObject description;
    public Artifact thisArtifact;
    // Start is called before the first frame update
    public void ShowDescription(GameObject artifact)
    {
        description = GameObject.Instantiate(descriptionPrefab, new Vector2(artifact.transform.position.x + 200, artifact.transform.position.y), Quaternion.identity);
        description.GetComponentInChildren<TMP_Text>().text = thisArtifact.description;
        description.transform.SetParent(artifact.transform);
    }

    public void HideDescription()
    {
        Destroy(description.gameObject);
    }
}
