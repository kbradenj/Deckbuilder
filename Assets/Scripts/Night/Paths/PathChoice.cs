using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class PathChoice : MonoBehaviour, IPointerClickHandler
{
    public TMP_Text pathText;
    public TMP_Text timeCostText;
    public string pathChoiceType;
    public int timeCost;
    public Singleton singleton;

    void Awake()
    {
        singleton = FindObjectOfType<Singleton>();
    }

    public virtual void AssignEvent()
    {
        var children = this.GetComponentsInChildren<TMP_Text>();
        foreach (var child in children)
        {
            if(child.name == "PathName")
            {
                pathText = child;
            }
            else
            {
                timeCostText = child;
            }
        }

        pathText.text = pathChoiceType;
        timeCostText.text = timeCost + " min";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Choice();
    }

    public virtual void Choice()
    { 
        singleton.AdjustMoonlight(timeCost);
    }
}
