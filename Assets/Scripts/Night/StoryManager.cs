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
    public Singleton singleton;

    //UI
    public TMP_Text storyText;
    public Image image;

    void Start()
    {
        singleton = GameObject.FindObjectOfType<Singleton>();
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
        storyText.text = chosenStory.storyWords;

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

    public void Choice(StoryOption option)
    {
    foreach(OptionEffect effect in option.effects)
        {
            switch(effect.effectKey)
            {
                case "strength":
                AddStrength(effect.amount);
                break;

                case "heal":
                Heal(effect.amount);
                break;

                case "randomCard":
                int randomIndex = Random.Range(0, singleton.cardDatabase.Count -1);
                AddCardToDeck(singleton.cardDatabase[randomIndex], effect.amount);
                break;

            }
        }
        GameObject.FindObjectOfType<Navigation>().Night();
    }

    void AddStrength(int amount)
    {
        singleton.player.baseStrength += amount;
    }

    void Heal(int amount)
    {
        if(singleton.player.health + amount > singleton.player.maxHealth){
            singleton.player.health = singleton.player.maxHealth;
        }
        else
        {
            singleton.player.health += amount;
        }

    }

    void AddCardToDeck(Card card, int amount)
    {
        for(int i = 0; i < amount; i++)
        {
               singleton.playerDeck.Add(card);
        }
     
    }




}
