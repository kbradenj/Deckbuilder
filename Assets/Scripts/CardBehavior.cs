using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CardBehavior : MonoBehaviour
{
    //TMP
    public TextMeshProUGUI cardNameField;
    public TextMeshProUGUI descriptionField;
    public TextMeshProUGUI costField;

    //UI
    public Image image;
    public Image bgImage;


    //Scripts
    public Player player;
    public Character targetCharacter;
    public Card card;
    public Actions actions;
    

    //Game Objects
    public GameObject Canvas;
    private GameObject startParent;
    private Vector2 startPosition;
    private GameObject dropZone;
    private GameObject target;
    private GameObject[] enemies;
    public GameObject inspectCardPrefab;


    //States
    private bool isDragging = false;
    private bool isOverDropZone = false;
    
    void Start()
    {
        Canvas = GameObject.Find("Main Canvas");
        dropZone = GameObject.Find("Drop Zone");
        player = FindObjectOfType<Player>();
        actions = FindObjectOfType<Actions>();
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Battle"){
            if(isDragging)
            {
                transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                transform.SetParent(Canvas.transform, true);
                if(!card.needsTarget && card.actionList[0] == "attackall"){
                    enemies = GameObject.FindGameObjectsWithTag("enemy");
                    foreach(GameObject enemy in enemies){
                        enemy.GetComponent<Enemy>().Highlight();
                    }
                }
            }
            IsCardPlayable();
        }
    }

    //Card Close-up
    public void CloseUp(){
       
            GameObject cardToDestroy = GameObject.FindGameObjectWithTag("Close Up");
            Destroy(cardToDestroy);
        if(!isDragging){
            GameObject closeUpCard = GameObject.Instantiate(inspectCardPrefab, new Vector2(Screen.width/2f,Screen.height/2f), Quaternion.identity) as GameObject;
            InspectCard inspectCard = closeUpCard.GetComponent<InspectCard>();
            inspectCard.RenderCard(card);
            closeUpCard.tag = "Close Up";
            closeUpCard.transform.SetParent(Canvas.transform);
        }
    }

    //Populate Card Data to UI
    public void RenderCard(Card c)
    {
        card = c;
        cardNameField.text = c.cardName;
        descriptionField.text = c.FormatString();
        if(c.actionList[0] == "xattack" || c.actionList[0] == "xblock"){
            costField.text = "X"; 
        }
        else{
            costField.text = c.cardCost.ToString(); 
        }
        image.sprite = c.cardImage;
    }


    //Card Interaction
    private void OnCollisionEnter2D(Collision2D collision)
    {
       isOverDropZone = true;
       if(card.needsTarget && collision.gameObject.tag == "enemy")
       {
       target = collision.gameObject;
       target.GetComponent<Enemy>().Highlight();
       }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropZone = false;
        if(target != null && target.tag == "enemy"){
         target.GetComponent<Enemy>().StopHighlight();
         target = null;
        }
    }

    public void StartDrag()
    {
        isDragging = true;
        startParent = transform.parent.gameObject;
        startPosition = transform.position;
    }

    public void EndDrag()
    {
        isDragging = false;
        if(isOverDropZone && IsCardPlayable())
        {
            if((card.needsTarget && target != null) || !card.needsTarget){
                
                if(card.actionList[0] == "attackall"){
                    foreach(GameObject enemy in enemies){
                        target = enemy;
                        Play(target);
                    }
                }
                else
                {
                Play(target);
                }



                player.turnAP -= card.cardCost;
                player.UpdateStats();
                Destroy(this.gameObject);
            }
            else
            {
                transform.position = startPosition;
                transform.SetParent(startParent.transform, false);
            }
        } 
        else
            {
                transform.position = startPosition;
                transform.SetParent(startParent.transform, false);
            }
        if(!card.needsTarget && card.actionList[0] == "attackall"){
                foreach(GameObject enemy in enemies){
                    enemy.GetComponent<Enemy>().StopHighlight();
                }
            }
    }

    public void Play(GameObject target)
    {   if(target == null){
            targetCharacter = player;  
        }
        else
        {
           targetCharacter = target.GetComponent<Character>(); 
        }

        foreach(string type in card.actionList)
        {
            switch(type)
            {
                case "attack":
                actions.Attack(targetCharacter, card.attack + player.strength, card.multiAction);
                break;
                case "xattack":
                actions.Attack(targetCharacter, card.attack, player.turnAP);
                player.turnAP = 0;
                break;
                case "attackall":
                actions.Attack(targetCharacter, card.attack, card.multiAction);
                break;
                case "block":
                if(target != null && target.tag == "enemy")
                {
                    actions.Block(player, card.block);
                }
                else
                {
                    actions.Block(targetCharacter, card.block);
                }
                break;
                case "xblock":
                for(int i = 0; i < player.turnAP; i++){
                    actions.Block(player, card.block);
                }
                player.turnAP = 0;
                break;
                case "vulnerable":
                actions.Vulnerable(targetCharacter, card.vulnerable);
                break;
                case "weak":
                actions.Weak(targetCharacter, card.weak);
                targetCharacter.NextTurn();
                break;
                case "strength":
                actions.Strength(targetCharacter, card.strength);
                targetCharacter.NextTurn();
                break;
            }
        }   
        targetCharacter.UpdateStats();
      
    }

    public bool IsCardPlayable()
    {
        if(player.turnAP - card.cardCost >= 0){
            bgImage.color = Color.green;
            return true;
        } else
        {
            bgImage.color = Color.black;
            return false;
        }
    }
}
