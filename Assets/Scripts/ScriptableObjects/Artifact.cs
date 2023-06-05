using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Artifact", menuName = "Artifact/Blank")]
public class Artifact : ScriptableObject
{
    public string description;
    public string phase;
    public bool isActive = false;

    public Sprite artifactImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public virtual void Effect() 
    {
        
    }

    public virtual void RemoveEffect() 
    {
        
    }
}
