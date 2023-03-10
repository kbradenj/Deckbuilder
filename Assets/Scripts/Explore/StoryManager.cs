using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryManager : MonoBehaviour
{
    //Lists
    public Dictionary<int, Story> stories;
    public Story[] storyDatabase;

    //Choise Storage
    public Story chosenStory;

    //Game Objects
    public GameObject storyOptionPrefab;
    public GameObject optionsArea;

    //UI
    public TMP_Text paragraphText;
    public Image image;

    void Start()
    {
       LoadStories();
       ChooseStory();
    }

    public void LoadStories()
    {
        stories = new Dictionary<int, Story>();
        storyDatabase = Resources.LoadAll<Story>("Stories");
        for(int i = 0; i < storyDatabase.Length; i++)
        {
            stories.Add(i, storyDatabase[i]);
        }
    }
    public void ChooseStory(){
        chosenStory = stories[0];
        DisplayStory();
    }

    public void DisplayStory(){
        paragraphText.text = "";
        StringBuilder sb = new StringBuilder();
        foreach(string paragraph in chosenStory.paragraphs)
        {   
            sb.AppendLine(paragraph + "\n");
        }
        paragraphText.text = sb.ToString();
        LoadOptions();
    }

    public void LoadOptions()
    {
        foreach(StoryOption option in chosenStory.storyOptions)
        {
            GameObject optionObject = Instantiate(storyOptionPrefab, Vector2.zero, Quaternion.identity);
            optionObject.transform.SetParent(optionsArea.transform);
            optionObject.GetComponent<StoryOptionBehavior>().storyOption = option;
        }
        

    }
}
