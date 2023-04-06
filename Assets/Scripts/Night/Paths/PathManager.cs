using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public GameObject pathChoicePrefab;
    public GameObject pathChoiceArea;

    public Singleton singleton;

    private List<string> tempChoices = new List<string>();
    private List<PathOption> pathOptions = new List<PathOption>();
    private Dictionary<int, PathOption> possiblePathChoices = new Dictionary<int, PathOption>();

    void Start()
    {
        singleton = FindObjectOfType<Singleton>();
        pathChoiceArea = GameObject.Find("Path Choices");
        GeneratePaths();
    }

    void GeneratePaths()
    {
        LoadPossiblePathChoices();
        tempChoices.Clear();
        int initialPossibleChoicesCount = possiblePathChoices.Count;
        int counter = 0;
        //Looking for 3 choices until the possibilities are exhausted
        while(tempChoices.Count < 3 && possiblePathChoices.Count != 0)
        {
            counter++;
            if(counter > 200)
            {
                Debug.Log("Counter max reached");
                return;
            }
            //random index of choices
            int randomIndex = Random.Range(0, initialPossibleChoicesCount);

            //has to be in the dictionary and cost less than the night left
            if(possiblePathChoices.ContainsKey(randomIndex))
            {
                if(possiblePathChoices[randomIndex].timeCost <= singleton.nightLeft)
                {
                    //grab the matching path from random num
                    PathOption randomEvent = possiblePathChoices[randomIndex];

                    //create path
                    RenderPath(randomEvent.pathChoiceType);
                    tempChoices.Add(randomEvent.pathChoiceType);
                }

                //remove guess from dictionary
                possiblePathChoices.Remove(randomIndex);
            }
        }
    }

    void LoadPossiblePathChoices()
    {
        possiblePathChoices.Clear();
        pathOptions.Add(new PathOption("easyEnemy", 60));
        pathOptions.Add(new PathOption("mediumEnemy", 120));
        pathOptions.Add(new PathOption("hardEnemy", 180));
        pathOptions.Add(new PathOption("story", 60));
        int counter = 0;
        foreach(PathOption pathOption in pathOptions)
        {
            possiblePathChoices.Add(counter, pathOption);
            counter++;
        }
    }

    void RenderPath(string path)
    {
        GameObject pathChoiceObject = GameObject.Instantiate(pathChoicePrefab, Vector2.zero, Quaternion.identity);
        pathChoiceObject.transform.SetParent(pathChoiceArea.transform);
        AttachPathScript(path, pathChoiceObject).AssignEvent();
    }

    PathChoice AttachPathScript(string path, GameObject pathObject)
    {
        switch(path)
        {
            case "easyEnemy":
            return pathObject.AddComponent<EasyEnemyPath>();
            case "mediumEnemy":
            return pathObject.AddComponent<MediumEnemyPath>();
            case "hardEnemy":
            return pathObject.AddComponent<HardEnemyPath>();
            case "story":
            return pathObject.AddComponent<StoryPath>();
            default:
            return pathObject.AddComponent<PathChoice>();
        }
    }

    class PathOption
    {
        public string pathChoiceType;
        public int timeCost;

        public PathOption(string x, int y)
        {
            pathChoiceType = x;
            timeCost = y;
        }
    }
}
