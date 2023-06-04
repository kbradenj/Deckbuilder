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
    public GameObject cardVisualPrefab;
    public GameObject optionsArea;
    public Singleton singleton;

    //UI
    public TMP_Text storyText;
    public TMP_Text rewardText;
    public GameObject rewardImageArea;
    public GameObject rewardView;
    public GameObject storyView;
    public Image image;

    //bools
    private bool isStoryView = true;

    void Start()
    {
        singleton = GameObject.FindObjectOfType<Singleton>();
        LoadStories();
        ChooseStory();
    }

    public void LoadStories()
    {
        storyDatabase = Resources.LoadAll<Story>("Stories");

    }
    public void ChooseStory(){
        chosenStory = storyDatabase[0];
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
    GameObject.Find("Story").SetActive(false);
    foreach(OptionEffect effect in option.effects)
        {
            switch(effect.effectKey)
            {
                case "strength":
                AddStrength(effect.amount);
                RewardSelectionText($"You have gained {effect.amount} {effect.effectKey} permanently!");
                break;

                case "heal":
                Heal(effect.amount);
                RewardSelectionText($"You have been healed for {effect.amount}!");
                break;

                case "randomCard":
                Card selectedCard = singleton.GetRandomAvailableCard();
                AddCardToDeck(selectedCard, effect.amount);
                ShowCardVisual(selectedCard);
                RewardSelectionText($"{selectedCard.cardName} has been added to your deck!");
                break;
            }
            ToggleView();
        }
    }

    void ShowCardVisual(Card card)
    {
        GameObject cardVisual = Instantiate(cardVisualPrefab, Vector2.zero, Quaternion.identity);
        cardVisual.transform.SetParent(rewardImageArea.transform);

        cardVisual.GetComponent<CardBehavior>().RenderCard(card);
    }

    void RewardSelectionText(string message)
    {
        rewardText.text = message;
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

    private void ToggleView()
    {
        if(isStoryView)
        {
            isStoryView = false;
            rewardView.SetActive(true);
            storyView.SetActive(false);
        }
        else
        {
            isStoryView = true;
            rewardView.SetActive(false);
            storyView.SetActive(true);
        }
    }
    public void ContinueNight()
    {
        singleton.navigation.Night();
    }




}
